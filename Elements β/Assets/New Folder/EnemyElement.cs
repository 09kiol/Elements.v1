using UnityEngine;

public class EnemyElement : MonoBehaviour
{ 
    public enum Element
{
    Fire,
    Water,
    Grass
}

public Element currentElement;
public int health = 100;
private bool isDead = false;

public delegate void EnemyDeathHandler();
public event EnemyDeathHandler OnEnemyDeath;

public void TakeDamage(int damage)
{
    if (isDead)
    {
        Debug.Log($"{gameObject.name}: 已死亡，无法再次受伤！");
        return;
    }

    health -= damage;
    Debug.Log($"{gameObject.name}: 受到 {damage} 点伤害，剩余血量：{health}");

    if (health <= 0)
    {
        Die(); 
    }
}

private void Die()
{
    if (isDead)
    {
        Debug.LogWarning($"{gameObject.name}: 已经标记为死亡，跳过销毁逻辑！");
        return;
    }

    isDead = true;
    Debug.Log($"{gameObject.name}: 死亡！执行销毁...");

    OnEnemyDeath?.Invoke();  

    if (gameObject != null)
    {
        Destroy(gameObject);
        Debug.Log($"{gameObject.name}: 已被销毁！");
    }
    else
    {
        Debug.LogError($"{gameObject.name}: 销毁失败，游戏对象为空！");
    }
}
}