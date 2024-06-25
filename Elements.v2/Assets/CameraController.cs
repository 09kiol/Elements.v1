using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 5f;
    public float followSpeed = 10f;
    public float sensitivity = 0.5f;

    private PlayerControls inputActions;
    private Vector2 lookInput;
    private Vector3 offset;

    private void Awake()
    {
        inputActions = new PlayerControls();
        offset = transform.position - player.position;
    }

    private void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Look.performed += OnLook;
        inputActions.Gameplay.Look.canceled += OnLook;
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Look.performed -= OnLook;
        inputActions.Gameplay.Look.canceled -= OnLook;
        inputActions.Gameplay.Disable();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        if (lookInput.sqrMagnitude > 0.01f)
        {
            // 反转水平输入
            float horizontalInput = -lookInput.x* sensitivity;

            Quaternion camTurnAngle = Quaternion.AngleAxis(horizontalInput * rotationSpeed, Vector3.up);
            offset = camTurnAngle * offset;

            Vector3 newPos = player.position + offset;
            transform.position = Vector3.Slerp(transform.position, newPos, Time.deltaTime * followSpeed);

            transform.LookAt(player);
        }
    }
}