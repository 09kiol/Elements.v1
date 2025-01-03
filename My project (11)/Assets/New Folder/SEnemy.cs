using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEnemy : MonoBehaviour
{
    public GameObject[] enemyPrefabs;    
    public int maxEnemies = 40;        
    public int totalEnemyLimit = 60;     

    private int currentEnemyCount = 0;   
    private int totalSpawnedEnemies = 0; 
    private List<GameObject> activeEnemies = new List<GameObject>(); 

    public Transform player;             
    public float playerSafeRadius = 5f; 
    private Bounds spawnBounds;          

    private void Start()
    {
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

        for (int i = 0; i < maxEnemies && totalSpawnedEnemies < totalEnemyLimit; i++)
        {
            SpawnEnemyRandomly();
        }
    }

    private void SpawnEnemyRandomly()
    {
        if (totalSpawnedEnemies >= totalEnemyLimit || currentEnemyCount >= maxEnemies)
        {
            Debug.LogWarning("敌人生成达到限制！");
            return;
        }

        int spawnAttempts = 10;
        while (spawnAttempts > 0)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnBounds.min.x, spawnBounds.max.x),
                Random.Range(spawnBounds.min.y, spawnBounds.max.y),
                Random.Range(spawnBounds.min.z, spawnBounds.max.z)
            );

            if (player != null && Vector3.Distance(randomPosition, player.position) < playerSafeRadius)
            {
                spawnAttempts--;
                continue;
            }

            Collider[] nearbyEnemies = Physics.OverlapSphere(randomPosition, 2f);
            bool isOccupied = false;

            foreach (var collider in nearbyEnemies)
            {
                if (collider.CompareTag("Enemy"))
                {
                    isOccupied = true;
                    break;
                }
            }

            if (!isOccupied)
            {
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyPrefab = enemyPrefabs[randomIndex];

                GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                currentEnemyCount++;
                totalSpawnedEnemies++;
                activeEnemies.Add(newEnemy);

                EnemyElement enemyElement = newEnemy.GetComponent<EnemyElement>();
                if (enemyElement != null)
                {
                    enemyElement.OnEnemyDeath += HandleEnemyDeath;
                    Debug.Log($"{newEnemy.name} 已生成，绑定了死亡事件。");
                }
                else
                {
                    Debug.LogError($"{newEnemy.name} 未找到 EnemyElement 脚本！");
                }

                return;
            }

            spawnAttempts--;
        }

        Debug.Log("随机生成失败，未找到合适位置。");
    }

    private void HandleEnemyDeath()
    {
        currentEnemyCount--;
        Debug.Log($"敌人死亡！当前敌人数量：{currentEnemyCount}，已生成总数：{totalSpawnedEnemies}");

        activeEnemies.RemoveAll(enemy => enemy == null); // 清理已销毁的敌人

        if (currentEnemyCount < maxEnemies && totalSpawnedEnemies < totalEnemyLimit)
        {
            SpawnEnemyRandomly();  // 补充新敌人
        }
    }
}