using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Att : MonoBehaviour
{
    private List<GameObject> enemiesInZone = new List<GameObject>();
    private PlayerControls inputActions;
    private PlayerElement playerElement;
    public int baseDamage = 25;

    private void Awake()
    {
        inputActions = new PlayerControls();
        playerElement = GetComponent<PlayerElement>();

        if (playerElement == null)
        {
            Debug.LogError("PLAYER ELEMENT");
        }
    }

    private void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Att.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Att.performed -= OnAttack;
        inputActions.Gameplay.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInZone.Remove(other.gameObject);
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (enemiesInZone.Count == 0)
        {
            Debug.Log("��ǰ������û�е��ˡ�");
            return;
        }

        List<GameObject> validEnemies = new List<GameObject>(enemiesInZone);

        foreach (var enemy in validEnemies)
        {
            if (enemy == null)
            {
                Debug.LogWarning("����һ�������ٵĵ��˶���������");
                continue;
            }

            EnemyElement enemyElement = enemy.GetComponent<EnemyElement>();
            if (enemyElement != null)
            {
                int damage = CalculateDamage(playerElement.currentElement, enemyElement.currentElement);

                if (damage > 0)
                {
                    enemyElement.TakeDamage(damage);
                    Debug.Log($"�Ե�������� {damage} ���˺�");
                }
                else
                {
                    Debug.Log("��ҵ��������ƣ��޷��Ե�������˺���");
                }
            }
            else
            {
                Debug.LogWarning("����δ���� EnemyElement �����");
            }
        }
    }

    private int CalculateDamage(PlayerElement.Element playerElement, EnemyElement.Element enemyElement)
    {
        switch (playerElement)
        {
            case PlayerElement.Element.Fire:
                return enemyElement == EnemyElement.Element.Grass ? baseDamage * 2 : (enemyElement == EnemyElement.Element.Fire ? baseDamage : 0);
            case PlayerElement.Element.Water:
                return enemyElement == EnemyElement.Element.Fire ? baseDamage * 2 : (enemyElement == EnemyElement.Element.Water ? baseDamage : 0);
            case PlayerElement.Element.Grass:
                return enemyElement == EnemyElement.Element.Water ? baseDamage * 2 : (enemyElement == EnemyElement.Element.Grass ? baseDamage : 0);
            default:
                return 0;
        }
    }
}