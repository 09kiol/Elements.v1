using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI; 

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float moveSensitivity = 0.5f;
    public int maxHealth = 3;
    private int currentHealth;
    public string targetSceneName;

    private PlayerControls inputActions;
    private Vector2 moveInput;
    private Rigidbody rb;
    private bool isGrounded;

    public Transform cameraTransform;

    private bool isInvincible = false;
    public float invincibleDuration = 1.0f;
    private Renderer playerRenderer;
    private Material playerMaterial;
    private Color originalColor;


    public Slider healthSlider;

    private void Awake()
    {
        inputActions = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        playerRenderer = GetComponent<Renderer>();
        playerMaterial = Instantiate(playerRenderer.material); 
        playerRenderer.material = playerMaterial;
        originalColor = playerMaterial.color;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Move.performed += OnMove;
        inputActions.Gameplay.Move.canceled += OnMove;
        inputActions.Gameplay.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Move.performed -= OnMove;
        inputActions.Gameplay.Move.canceled -= OnMove;
        inputActions.Gameplay.Jump.performed -= OnJump;
        inputActions.Gameplay.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>() * moveSensitivity;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0;
        right.Normalize();

        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;
        Vector3 targetPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(targetRotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isInvincible)
        {
            TakeDamage(1);
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; 
        }

        Debug.Log($"Player HP -{damage}, 当前HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float elapsed = 0f;

        while (elapsed < invincibleDuration)
        {
            Color newColor = originalColor;
            newColor.a = Mathf.Approximately(newColor.a, 1f) ? 0.5f : 1f; 
            playerMaterial.color = newColor;

            yield return new WaitForSeconds(0.2f); 
            elapsed += 0.2f;
        }

        playerMaterial.color = originalColor; 
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("Player Died");
        StartCoroutine(ReloadSceneAfterDelay());
    }

    private IEnumerator ReloadSceneAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(targetSceneName);
    }
}
