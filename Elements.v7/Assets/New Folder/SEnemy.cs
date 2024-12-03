using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEnemy : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // ���ֵ���Ԥ����
    public GameObject bossPrefab;     // BossԤ����
    public int maxEnemies = 40;       // ��ǰͬʱ���ڵĵ�������
    public int totalEnemyLimit = 60;  // �������ɵ�����������
    private int currentEnemyCount = 0;    // ��ǰ��ĵ�������
    private int totalSpawnedEnemies = 0;  // �Ѿ����ɵĵ�������
    private int totalDeadEnemies = 0;     // �������ĵ�������
    private bool bossSpawned = false;     // ���Boss�Ƿ�������
    private List<GameObject> activeEnemies = new List<GameObject>(); // ���ĵ����б�

    public Transform player;              // ��� Transform
    public float playerSafeRadius = 5f;   // ��Ҹ����İ�ȫ����
    private Bounds spawnBounds;           // �洢����ģ�͵ı߽�

    private void Start()
    {
        // ��ȡ������ı߽緶Χ
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

        // ��ʼʱ���ɳ�ʼ�����ĵ���
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

        int spawnAttempts = 10; // ��ೢ��10������λ��
        while (spawnAttempts > 0)
        {
            // ������߽緶Χ���������λ��
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnBounds.min.x, spawnBounds.max.x),
                Random.Range(spawnBounds.min.y, spawnBounds.max.y),
                Random.Range(spawnBounds.min.z, spawnBounds.max.z)
            );

            // ������λ������ҵľ���
            if (player != null && Vector3.Distance(randomPosition, player.position) < playerSafeRadius)
            {
                spawnAttempts--;
                continue; // ����̫�������³���
            }

            // ������λ�ø����Ƿ�����������
            Collider[] nearbyEnemies = Physics.OverlapSphere(randomPosition, 2f); // 2fΪ���뾶
            bool isOccupied = false;

            foreach (var collider in nearbyEnemies)
            {
                if (collider.CompareTag("Enemy")) // ������˶�����"Enemy"��ǩ
                {
                    isOccupied = true;
                    break;
                }
            }

            if (!isOccupied)
            {
                // ���ѡ��һ������Ԥ����
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyPrefab = enemyPrefabs[randomIndex];

                // ���ɵ���
                GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                currentEnemyCount++;
                totalSpawnedEnemies++;
                activeEnemies.Add(newEnemy);

                // �������������¼�
                EnemyElement enemyElement = newEnemy.GetComponent<EnemyElement>();
                if (enemyElement != null)
                {
                    enemyElement.OnEnemyDeath += HandleEnemyDeath;
                }
                return; // �ɹ����ɵ��˺��˳�ѭ��
            }

            spawnAttempts--;
        }

        Debug.Log("�������ʧ�ܣ�δ�ҵ�����λ�á�");
    }

    private void HandleEnemyDeath()
    {
        currentEnemyCount--;
        totalDeadEnemies++;  // ��������������Ŀ

        // �Ƴ��Ѿ������ĵ���
        activeEnemies.RemoveAll(enemy => enemy == null);

        // ����Ƿ�ﵽ70%��������������Boss
        if (!bossSpawned && totalDeadEnemies >= totalEnemyLimit * 0.7f)
        {
            SpawnBoss();
        }

        // �����û�дﵽ�������ɵ������ƣ��ҵ�ǰ��������û�г������ޣ��������µĵ���
        if (currentEnemyCount < maxEnemies && totalSpawnedEnemies < totalEnemyLimit)
        {
            SpawnEnemyRandomly();
        }
    }

    private void SpawnBoss()
    {
        bossSpawned = true; // ���Boss�Ѿ�����

        // ������߽���������Boss
        Vector3 bossPosition = spawnBounds.center;
        Instantiate(bossPrefab, bossPosition, Quaternion.identity);
        Debug.Log("Boss����!");
    }
}
