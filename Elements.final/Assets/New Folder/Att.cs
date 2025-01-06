using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Att : MonoBehaviour
{
    private List<GameObject> enemiesInZone = new List<GameObject>();
    private PlayerControls inputActions;
    private PlayerElement playerElement;

    public int baseDamage = 25;
    private Animator animator; // 动画控制器
    public float attackCooldown = 0.5f; // 攻击冷却时间
    private bool isAttacking = false; // 标记是否正在攻击
    private float lastAttackTime = 0; // 上次攻击时间

    private void Awake()
    {
        inputActions = new PlayerControls();
        playerElement = GetComponent<PlayerElement>();
        animator = GetComponent<Animator>();

        if (playerElement == null)
        {
            Debug.LogError("PLAYER ELEMENT 未找到！");
        }

        if (animator == null)
        {
            Debug.LogError("Animator 未找到！");
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
        if (Time.time < lastAttackTime + attackCooldown)
        {
            Debug.Log("攻击冷却中...");
            return;
        }

        if (enemiesInZone.Count == 0)
        {
            Debug.Log("当前区域内没有敌人。");
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        lastAttackTime = Time.time;
        isAttacking = true;

        StartCoroutine(PerformAttack());
    }

    private System.Collections.IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(0.2f); 

        List<GameObject> validEnemies = new List<GameObject>(enemiesInZone);

        foreach (var enemy in validEnemies)
        {
            if (enemy == null)
            {
                Debug.LogWarning("发现一个已销毁的敌人对象，跳过。");
                continue;
            }

            EnemyElement enemyElement = enemy.GetComponent<EnemyElement>();
            if (enemyElement != null)
            {
                int damage = CalculateDamage(playerElement.currentElement, enemyElement.currentElement);

                if (damage > 0)
                {
                    enemyElement.TakeDamage(damage);
                    Debug.Log($"对敌人造成了 {damage} 点伤害");
                }
                else
                {
                    Debug.Log("玩家的属性劣势，无法对敌人造成伤害！");
                }
            }
            else
            {
                Debug.LogWarning("敌人未包含 EnemyElement 组件！");
            }
        }

        isAttacking = false; // 攻击结束，恢复状态
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