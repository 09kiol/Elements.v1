using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLPlayer : MonoBehaviour
{
    public Transform player; // ��Ҷ����Transform���

    void Update()
    {
        if (player != null)
        {
            // �÷����λ�õ�����ҵ�λ�ã����Ը�����Ҫ����ƫ����
            transform.position = player.position;
        }
    }
}
