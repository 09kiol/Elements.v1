using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public InputActionAsset inputActions;
    private InputAction lookAction;

    public float rotationSpeed = 5f;

    private void Awake()
    {
        if (freeLookCamera == null)
        {
            freeLookCamera = GetComponent<CinemachineFreeLook>();
        }

        var gameplayMap = inputActions.FindActionMap("Gameplay");
        lookAction = gameplayMap.FindAction("Look");

        lookAction.Enable();
    }

    private void LateUpdate()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        if (lookInput.x != 0)
        {
            // 只控制X轴，水平旋转相机
            freeLookCamera.m_XAxis.Value += lookInput.x * rotationSpeed * Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        lookAction.Disable();
    }
}