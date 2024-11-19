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
    public Material playerMaterial; // 拖入的材质，在Inspector中直接设置

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
        if (playerMaterial != null)
        {
            playerMaterial.color = Color.red; // 设置为红色
            Debug.Log("当前元素: Fire");
        }
    }

    private void OnChangeToWater(InputAction.CallbackContext context)
    {
        currentElement = Element.Water;
        if (playerMaterial != null)
        {
            playerMaterial.color = Color.blue; // 设置为蓝色
            Debug.Log("当前元素: Water");
        }
    }

    private void OnChangeToGrass(InputAction.CallbackContext context)
    {
        currentElement = Element.Grass;
        if (playerMaterial != null)
        {
            playerMaterial.color = Color.green; // 设置为绿色
            Debug.Log("当前元素: Grass");
        }
    }
}
