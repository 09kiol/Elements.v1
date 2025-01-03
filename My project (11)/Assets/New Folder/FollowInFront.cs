using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowInFront : MonoBehaviour
{
    public Transform player;         
    public float distance = 2f;      
    public float heightOffset = 1f;  
    public float followSpeed = 5f;   
    public bool facePlayer = false;

    private void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("未找到玩家对象！");
            return;
        }

        Vector3 targetPosition = player.position + player.forward * distance + Vector3.up * heightOffset;


        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        if (facePlayer)
        {
            transform.LookAt(player);
        }
        else
        {
            transform.rotation = player.rotation;
        }
    }
}