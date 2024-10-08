using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEnemy : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // 三种敌人预制体
    public int maxEnemies = 40;       // 当前同时存在的敌人上限
    public int totalEnemyLimit = 60;  // 敌人生成的总数量限制
    private int currentEnemyCount = 0;    // 当前活动的敌人数量
    private int totalSpawnedEnemies = 0;  // 已经生成的敌人总数
    public Transform[] spawnPoints;   // 刷新的地点

    private List<GameObject> activeEnemies = new List<GameObject>(); // 存活的敌人列表

    private void Start()
    {
        // 开始时生成初始数量的敌人
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // 如果已生成的敌人数量达到总限制，则不再生成新的敌人
        if (totalSpawnedEnemies >= totalEnemyLimit)
        {
            return;
        }

        if (currentEnemyCount >= maxEnemies)
        {
            return; // 达到当前同时存在的敌人上限，不再生成
        }

        // 随机选择一个敌人预制体
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];

        // 随机选择一个生成点
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // 生成敌人
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        currentEnemyCount++;
        totalSpawnedEnemies++;  // 增加已生成的敌人数目
        activeEnemies.Add(newEnemy);

        // 监听敌人死亡事件
        EnemyElement enemyElement = newEnemy.GetComponent<EnemyElement>();
        if (enemyElement != null)
        {
            enemyElement.OnEnemyDeath += HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath()
    {
        currentEnemyCount--;

        // 移除已经死亡的敌人
        activeEnemies.RemoveAll(enemy => enemy == null);

        // 如果还没有达到敌人生成的总限制，且当前敌人数量没有超过上限，则生成新的敌人
        if (currentEnemyCount < maxEnemies && totalSpawnedEnemies < totalEnemyLimit)
        {
            SpawnEnemy();
        }
    }
}
