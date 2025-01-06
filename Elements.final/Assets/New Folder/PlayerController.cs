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

    private float lastAttackTime = -1f;
    private float attackCooldown = 0.6f;

    private PlayerControls inputActions;
    private Vector2 moveInput;
    private Rigidbody rb;
    private bool isGrounded;

    private Animator animator;

    public Transform cameraTransform;

    private bool isInvincible = false;
    public float invincibleDuration = 1.0f;
    private Renderer playerRenderer;
    private Material playerMaterial;
    private Color originalColor;

    public Slider healthSlider;

    private bool isAlive = true;
    public float cameraInfluence = 1f;
    public float rotationSpeed = 100f;

    private void Awake()
    {
        inputActions = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerRenderer = GetComponent<Renderer>();

        if (maxHealth <= 0) maxHealth = 3;
        currentHealth = maxHealth;

        playerMaterial = Instantiate(playerRenderer.material);
        playerRenderer.material = playerMaterial;
        originalColor = playerMaterial.color;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Debug.Log($"[Awake] Player Initialized: maxHealth={maxHealth}, currentHealth={currentHealth}");
    }

    private void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Move.performed += OnMove;
        inputActions.Gameplay.Move.canceled += OnMove;
        inputActions.Gameplay.Jump.performed += OnJump;
        inputActions.Gameplay.Att.performed += OnAttack;

    }

    private void OnDisable()
    {
        inputActions.Gameplay.Move.performed -= OnMove;
        inputActions.Gameplay.Move.canceled -= OnMove;
        inputActions.Gameplay.Jump.performed -= OnJump;
        inputActions.Gameplay.Att.performed -= OnAttack;
        inputActions.Gameplay.Disable();

        Debug.LogWarning("PlayerController has been disabled!");
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (isAlive)
        {
            moveInput = context.ReadValue<Vector2>() * moveSensitivity;
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded && isAlive)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetTrigger("Jump");
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!isAlive) return;

        if (Time.time - lastAttackTime < attackCooldown)
        {
            Debug.Log("Attack is on cooldown!");
            return;
        }

        animator.SetTrigger("Attack");
        Debug.Log("Attack animation triggered!");

        lastAttackTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 selfForward = transform.forward;
        selfForward.y = 0;
        selfForward.Normalize();

        Vector3 selfRight = transform.right;
        selfRight.y = 0;
        selfRight.Normalize();

        Vector3 mixedForward = Vector3.Lerp(selfForward, camForward, cameraInfluence).normalized;
        Vector3 mixedRight = Vector3.Lerp(selfRight, camRight, cameraInfluence).normalized;

        Vector3 moveDirection = (mixedForward * moveInput.y + mixedRight * moveInput.x).normalized;

        Vector3 targetPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            float maxStep = rotationSpeed * Time.fixedDeltaTime;

            Quaternion smoothedRotation = Quaternion.RotateTowards(rb.rotation, targetRotation, maxStep);

            rb.MoveRotation(smoothedRotation);
        }

        float animationSpeed = moveDirection.magnitude * moveSpeed;
        animator.SetFloat("Speed", animationSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[OnTriggerEnter] Collided with: {other.gameObject.name}");
        if (other.CompareTag("Enemy") && !isInvincible)
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;

        Debug.Log($"[TakeDamage] Player takes damage: {damage}, Current Health Before: {currentHealth}");
        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            isAlive = false;
            Die();
        }
        Debug.Log($"[TakeDamage] Current Health After: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log("[Die] Player Died.");

        animator.SetTrigger("Die");

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        StartCoroutine(ReloadSceneAfterDelay(3f));
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(targetSceneName);
    }
}