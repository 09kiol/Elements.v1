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
        Debug.Log($"{gameObject.name}: ���������޷��ٴ����ˣ�");
        return;
    }

    health -= damage;
    Debug.Log($"{gameObject.name}: �ܵ� {damage} ���˺���ʣ��Ѫ����{health}");

    if (health <= 0)
    {
        Die(); 
    }
}

private void Die()
{
    if (isDead)
    {
        Debug.LogWarning($"{gameObject.name}: �Ѿ����Ϊ���������������߼���");
        return;
    }

    isDead = true;
    Debug.Log($"{gameObject.name}: ������ִ������...");

    OnEnemyDeath?.Invoke();  

    if (gameObject != null)
    {
        Destroy(gameObject);
        Debug.Log($"{gameObject.name}: �ѱ����٣�");
    }
    else
    {
        Debug.LogError($"{gameObject.name}: ����ʧ�ܣ���Ϸ����Ϊ�գ�");
    }
}
}