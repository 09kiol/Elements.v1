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
        Debug.Log("��ǰԪ��: ��");
    }

    private void OnChangeToWater(InputAction.CallbackContext context)
    {
        currentElement = Element.Water;
        Debug.Log("��ǰԪ��: ˮ");
    }

    private void OnChangeToGrass(InputAction.CallbackContext context)
    {
        currentElement = Element.Grass;
        Debug.Log("��ǰԪ��: ��");
    }
}