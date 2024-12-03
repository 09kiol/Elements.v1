using UnityEngine;

public class EnemyElement : MonoBehaviour
{
    public enum Element
    {
        Fire,
        Water,
        Grass
    }

    public Element currentElement;  // 敌人的当前元素
    public int health = 100;  // 敌人的血量

    // 敌人死亡事件
    public delegate void EnemyDeathHandler();
    public event EnemyDeathHandler OnEnemyDeath;

    // 受到伤害
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("敌人受到伤害: " + damage + ", 剩余血量: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die() //敌人死亡处理
    {
        Debug.Log("敌人死亡");

        // 触发死亡事件
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath();
        }

        Destroy(gameObject);
    }
}
