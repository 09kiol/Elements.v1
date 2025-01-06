using UnityEngine;

public class BossElement : MonoBehaviour
{
    public enum Element
    {
        Fire,
        Water,
        Grass
    }

    public Element currentElement = Element.Fire; // Boss��ʼΪ��Ԫ��
    public int health = 200;  // Boss����Ѫ��������200
    private bool hasChangedElement = false;  // ����Ƿ��Ѿ�ת��Ԫ��

    // Boss�����¼�
    public delegate void BossDeathHandler();
    public event BossDeathHandler OnBossDeath;

    // �ܵ��˺�
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Boss�ܵ��˺�: " + damage + ", ʣ��Ѫ��: " + health);

        // �������ֵ�Ƿ���벢��Ԫ����δ����
        if (health <= 100 && !hasChangedElement)
        {
            ChangeElement();
        }

        if (health <= 0)
        {
            Die();
        }
    }

    // Ԫ��ת���߼�
    private void ChangeElement()
    {
        hasChangedElement = true; 
        currentElement = (Random.value > 0.5f) ? Element.Grass : Element.Water;
        Debug.Log("Boss��Ԫ�ر��Ϊ: " + currentElement.ToString());
    }

    // Boss��������
    private void Die()
    {
        Debug.Log("Boss����");

        // ���������¼�
        if (OnBossDeath != null)
        {
            OnBossDeath();
        }

        Destroy(gameObject);
    }
}
