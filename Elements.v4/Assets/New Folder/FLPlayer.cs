using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLPlayer : MonoBehaviour
{
    public Transform player; // 玩家对象的Transform组件

    void Update()
    {
        if (player != null)
        {
            // 让方块的位置等于玩家的位置，可以根据需要调整偏移量
            transform.position = player.position;
        }
    }
}
