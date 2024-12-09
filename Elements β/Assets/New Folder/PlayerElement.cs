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

    public Color fireColor = Color.red;
    public Color waterColor = Color.blue;
    public Color grassColor = Color.green;

    private PlayerControls inputActions;

    private void Awake()
    {
        inputActions = new PlayerControls();

        if (playerMaterial == null)
        {
            Debug.LogWarning("PlayerMaterial δ���ã����� Inspector �з���һ�����ʡ�");
        }
    }

    private void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.SwitchNext.performed += OnSwitchNext; // ��RB
        inputActions.Gameplay.SwitchPrevious.performed += OnSwitchPrevious; // ��LB
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
                Debug.Log("��ǰԪ��: Fire");
                break;
            case Element.Water:
                playerMaterial.color = waterColor;
                Debug.Log("��ǰԪ��: Water");
                break;
            case Element.Grass:
                playerMaterial.color = grassColor;
                Debug.Log("��ǰԪ��: Grass");
                break;
        }
    }
}