using UnityEngine;

public class BossElement : MonoBehaviour
{
    public enum Element
    {
        Fire,
        Water,
        Grass
    }

    public Element currentElement = Element.Fire; // Boss初始为火元素
    public int health = 200;  // Boss的总血量，假设200
    private bool hasChangedElement = false;  // 标记是否已经转换元素

    // Boss死亡事件
    public delegate void BossDeathHandler();
    public event BossDeathHandler OnBossDeath;

    // 受到伤害
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Boss受到伤害: " + damage + ", 剩余血量: " + health);

        // 检查生命值是否减半并且元素尚未更改
        if (health <= 100 && !hasChangedElement)
        {
            ChangeElement();
        }

        if (health <= 0)
        {
            Die();
        }
    }

    // 元素转换逻辑
    private void ChangeElement()
    {
        hasChangedElement = true; 
        currentElement = (Random.value > 0.5f) ? Element.Grass : Element.Water;
        Debug.Log("Boss的元素变更为: " + currentElement.ToString());
    }

    // Boss死亡处理
    private void Die()
    {
        Debug.Log("Boss死亡");

        // 触发死亡事件
        if (OnBossDeath != null)
        {
            OnBossDeath();
        }

        Destroy(gameObject);
    }
}
