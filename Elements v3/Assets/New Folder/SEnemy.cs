using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEnemy : MonoBehaviour
{
    public GameObject prefabToSpawn; // 需要生成的预制体
    public int numberOfPrefabs = 5;  // 预制体的总数量
    public float spawnInterval = 1f; // 生成间隔
    public float spawnRadius = 5f;   // 生成预制体的半径

    private Transform player;        // 玩家的Transform
    private int prefabsSpawned = 0;  // 已生成的预制体数量
    private float timer = 0f;        // 计时器

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 找到标签为"Player"的游戏对象
    }

    void Update()
    {
        // 计算玩家和目标物体之间的距离
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 如果玩家接近，逐步生成预制体
        if (distanceToPlayer < spawnRadius && prefabsSpawned < numberOfPrefabs)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                // 随机生成位置
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;

                // 实例化预制体
                Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

                // 计数器加一
                prefabsSpawned++;
                timer = 0f; // 重置计时器
            }
        }
    }
}