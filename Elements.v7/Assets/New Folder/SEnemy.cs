using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEnemy : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // 三种敌人预制体
    public GameObject bossPrefab;     // Boss预制体
    public int maxEnemies = 40;       // 当前同时存在的敌人上限
    public int totalEnemyLimit = 60;  // 敌人生成的总数量限制
    private int currentEnemyCount = 0;    // 当前活动的敌人数量
    private int totalSpawnedEnemies = 0;  // 已经生成的敌人总数
    private int totalDeadEnemies = 0;     // 已死亡的敌人数量
    private bool bossSpawned = false;     // 标记Boss是否已生成
    private List<GameObject> activeEnemies = new List<GameObject>(); // 存活的敌人列表

    public Transform player;              // 玩家 Transform
    public float playerSafeRadius = 5f;   // 玩家附近的安全距离
    private Bounds spawnBounds;           // 存储物体模型的边界

    private void Start()
    {
        // 获取绑定物体的边界范围
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            spawnBounds = meshRenderer.bounds;
        }
        else
        {
            Debug.LogError("物体上没有找到 MeshRenderer，无法确定生成区域！");
            return;
        }

        // 开始时生成初始数量的敌人
        for (int i = 0; i < maxEnemies && totalSpawnedEnemies < totalEnemyLimit; i++)
        {
            SpawnEnemyRandomly();
        }
    }

    private void SpawnEnemyRandomly()
    {
        if (totalSpawnedEnemies >= totalEnemyLimit || currentEnemyCount >= maxEnemies)
        {
            return;
        }

        int spawnAttempts = 10; // 最多尝试10次生成位置
        while (spawnAttempts > 0)
        {
            // 在物体边界范围内随机生成位置
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnBounds.min.x, spawnBounds.max.x),
                Random.Range(spawnBounds.min.y, spawnBounds.max.y),
                Random.Range(spawnBounds.min.z, spawnBounds.max.z)
            );

            // 检查随机位置与玩家的距离
            if (player != null && Vector3.Distance(randomPosition, player.position) < playerSafeRadius)
            {
                spawnAttempts--;
                continue; // 距离太近，重新尝试
            }

            // 检查随机位置附近是否有其他敌人
            Collider[] nearbyEnemies = Physics.OverlapSphere(randomPosition, 2f); // 2f为检测半径
            bool isOccupied = false;

            foreach (var collider in nearbyEnemies)
            {
                if (collider.CompareTag("Enemy")) // 假设敌人都带有"Enemy"标签
                {
                    isOccupied = true;
                    break;
                }
            }

            if (!isOccupied)
            {
                // 随机选择一个敌人预制体
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyPrefab = enemyPrefabs[randomIndex];

                // 生成敌人
                GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                currentEnemyCount++;
                totalSpawnedEnemies++;
                activeEnemies.Add(newEnemy);

                // 监听敌人死亡事件
                EnemyElement enemyElement = newEnemy.GetComponent<EnemyElement>();
                if (enemyElement != null)
                {
                    enemyElement.OnEnemyDeath += HandleEnemyDeath;
                }
                return; // 成功生成敌人后退出循环
            }

            spawnAttempts--;
        }

        Debug.Log("随机生成失败，未找到合适位置。");
    }

    private void HandleEnemyDeath()
    {
        currentEnemyCount--;
        totalDeadEnemies++;  // 增加死亡敌人数目

        // 移除已经死亡的敌人
        activeEnemies.RemoveAll(enemy => enemy == null);

        // 检查是否达到70%死亡条件，生成Boss
        if (!bossSpawned && totalDeadEnemies >= totalEnemyLimit * 0.7f)
        {
            SpawnBoss();
        }

        // 如果还没有达到敌人生成的总限制，且当前敌人数量没有超过上限，则生成新的敌人
        if (currentEnemyCount < maxEnemies && totalSpawnedEnemies < totalEnemyLimit)
        {
            SpawnEnemyRandomly();
        }
    }

    private void SpawnBoss()
    {
        bossSpawned = true; // 标记Boss已经生成

        // 在物体边界中心生成Boss
        Vector3 bossPosition = spawnBounds.center;
        Instantiate(bossPrefab, bossPosition, Quaternion.identity);
        Debug.Log("Boss生成!");
    }
}
