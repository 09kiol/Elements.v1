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

    public Color fireColor = Color.red;
    public Color waterColor = Color.blue;
    public Color grassColor = Color.green;

    private PlayerControls inputActions;

    private void Awake()
    {
        inputActions = new PlayerControls();

        if (playerMaterial == null)
        {
            Debug.LogWarning("PlayerMaterial 未设置！请在 Inspector 中分配一个材质。");
        }
    }

    private void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.SwitchNext.performed += OnSwitchNext; // 绑定RB
        inputActions.Gameplay.SwitchPrevious.performed += OnSwitchPrevious; // 绑定LB
    }

    private void OnDisable()
    {
        inputActions.Gameplay.SwitchNext.performed -= OnSwitchNext;
        inputActions.Gameplay.SwitchPrevious.performed -= OnSwitchPrevious;
        inputActions.Gameplay.Disable();
    }

    private void OnSwitchNext(InputAction.CallbackContext context)
    {
        currentElement = (Element)(((int)currentElement + 1) % System.Enum.GetValues(typeof(Element)).Length);
        UpdateElementVisual();
    }

    private void OnSwitchPrevious(InputAction.CallbackContext context)
    {
        currentElement = (Element)(((int)currentElement - 1 + System.Enum.GetValues(typeof(Element)).Length) % System.Enum.GetValues(typeof(Element)).Length);
        UpdateElementVisual();
    }

    private void UpdateElementVisual()
    {
        switch (currentElement)
        {
            case Element.Fire:
                playerMaterial.color = fireColor;
                Debug.Log("当前元素: Fire");
                break;
            case Element.Water:
                playerMaterial.color = waterColor;
                Debug.Log("当前元素: Water");
                break;
            case Element.Grass:
                playerMaterial.color = grassColor;
                Debug.Log("当前元素: Grass");
                break;
        }
    }
}