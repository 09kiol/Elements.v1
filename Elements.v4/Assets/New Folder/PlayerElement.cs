using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerElement : MonoBehaviour
{
    public enum Element
    {
        Fire,   // 火
        Water,  // 水
        Grass   // 草
    }

    public Element currentElement;  // 当前元素

    private PlayerControls inputActions;

    private void Awake()
    {
        inputActions = new PlayerControls();
        
    }

    private void OnEnable()
    {
        inputActions.Gameplay.Enable();  // 启用Gameplay Action Map
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
        Debug.Log("当前元素: 火");
    }

    private void OnChangeToWater(InputAction.CallbackContext context)
    {
        currentElement = Element.Water;
        Debug.Log("当前元素: 水");
    }

    private void OnChangeToGrass(InputAction.CallbackContext context)
    {
        currentElement = Element.Grass;
        Debug.Log("当前元素: 草");
    }
}
