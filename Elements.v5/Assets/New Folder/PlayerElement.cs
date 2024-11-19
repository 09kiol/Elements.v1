using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerElement : MonoBehaviour
{
    public enum Element
    {
        Fire,   // ��
        Water,  // ˮ
        Grass   // ��
    }

    public Element currentElement;  // ��ǰԪ��
    public Material playerMaterial; // ����Ĳ��ʣ���Inspector��ֱ������

    private PlayerControls inputActions;

    private void Awake()
    {
        inputActions = new PlayerControls();
    }

    private void OnEnable()
    {
        inputActions.Gameplay.Enable();  // ����Gameplay Action Map
        inputActions.Gameplay.ChangeToFire.performed += OnChangeToFire;
        inputActions.Gameplay.ChangeToWater.performed += OnChangeToWater;
        inputActions.Gameplay.ChangeToGrass.performed += OnChangeToGrass;
    }

    private void OnDisable()
    {
        inputActions.Gameplay.ChangeToFire.performed -= OnChangeToFire;
        inputActions.Gameplay.ChangeToWater.performed -= OnChangeToWater;
        inputActions.Gameplay.ChangeToGrass.performed -= OnChangeToGrass;
        inputActions.Gameplay.Disable();
    }

    private void OnChangeToFire(InputAction.CallbackContext context)
    {
        currentElement = Element.Fire;
        if (playerMaterial != null)
        {
            playerMaterial.color = Color.red; // ����Ϊ��ɫ
            Debug.Log("��ǰԪ��: Fire");
        }
    }

    private void OnChangeToWater(InputAction.CallbackContext context)
    {
        currentElement = Element.Water;
        if (playerMaterial != null)
        {
            playerMaterial.color = Color.blue; // ����Ϊ��ɫ
            Debug.Log("��ǰԪ��: Water");
        }
    }

    private void OnChangeToGrass(InputAction.CallbackContext context)
    {
        currentElement = Element.Grass;
        if (playerMaterial != null)
        {
            playerMaterial.color = Color.green; // ����Ϊ��ɫ
            Debug.Log("��ǰԪ��: Grass");
        }
    }
}
