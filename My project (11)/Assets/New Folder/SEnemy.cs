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
            Debug.LogError("������û���ҵ� MeshRenderer���޷�ȷ����������");
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
            Debug.LogWarning("�������ɴﵽ���ƣ�");
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
                    Debug.Log($"{newEnemy.name} �����ɣ����������¼���");
                }
                else
                {
                    Debug.LogError($"{newEnemy.name} δ�ҵ� EnemyElement �ű���");
                }

                return;
            }

            spawnAttempts--;
        }

        Debug.Log("�������ʧ�ܣ�δ�ҵ�����λ�á�");
    }

    private void HandleEnemyDeath()
    {
        currentEnemyCount--;
        Debug.Log($"������������ǰ����������{currentEnemyCount}��������������{totalSpawnedEnemies}");

        activeEnemies.RemoveAll(enemy => enemy == null); // ���������ٵĵ���

        if (currentEnemyCount < maxEnemies && totalSpawnedEnemies < totalEnemyLimit)
        {
            SpawnEnemyRandomly();  // �����µ���
        }
    }
}