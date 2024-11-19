using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;  // 玩家角色的 Transform
    public float rotationSpeed = 100.0f;  // 摄像头旋转速度

    private PlayerControls inputActions;
    private Vector2 lookInput;

    void Awake()
    {
        inputActions = new PlayerControls();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Gameplay.Look.performed += OnLookPerformed;
        inputActions.Gameplay.Look.canceled += OnLookCanceled;
    }

    void OnDisable()
    {
        inputActions.Gameplay.Look.performed -= OnLookPerformed;
        inputActions.Gameplay.Look.canceled -= OnLookCanceled;
        inputActions.Disable();
    }

    void OnLookPerformed(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void OnLookCanceled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    void Update()
    {
        float desiredRotationAngle = lookInput.x * rotationSpeed * Time.deltaTime;

        // 围绕玩家角色旋转摄像头
        transform.RotateAround(player.position, Vector3.up, desiredRotationAngle);

        // 保持摄像头始终看向玩家
        transform.LookAt(player);
    }
}