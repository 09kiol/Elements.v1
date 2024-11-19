using UnityEngine;

public class EnemyElement : MonoBehaviour
{
    public enum Element
    {
        Fire,
        Water,
        Grass
    }

    public Element currentElement;  // ���˵ĵ�ǰԪ��
    public int health = 100;  // ���˵�Ѫ��

    // ���������¼�
    public delegate void EnemyDeathHandler();
    public event EnemyDeathHandler OnEnemyDeath;

    // �ܵ��˺�
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("�����ܵ��˺�: " + damage + ", ʣ��Ѫ��: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die() //������������
    {
        Debug.Log("��������");

        // ���������¼�
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath();
        }

        Destroy(gameObject);
    }
}
