using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;  // ��ҽ�ɫ�� Transform
    public float rotationSpeed = 100.0f;  // ����ͷ��ת�ٶ�

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

        // Χ����ҽ�ɫ��ת����ͷ
        transform.RotateAround(player.position, Vector3.up, desiredRotationAngle);

        // ��������ͷʼ�տ������
        transform.LookAt(player);
    }
}