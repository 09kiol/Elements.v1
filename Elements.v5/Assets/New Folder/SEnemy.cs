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
    public Transform[] spawnPoints;   // ˢ�µĵص�
    public Transform bossSpawnPoint;  // Boss���ɵص�
    private bool bossSpawned = false; // ���Boss�Ƿ�������

    private List<GameObject> activeEnemies = new List<GameObject>(); // ���ĵ����б�

    private void Start()
    {
        // ��ʼʱ���ɳ�ʼ�����ĵ���
        for (int i = 0; i < maxEnemies && totalSpawnedEnemies < totalEnemyLimit; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // ��������ɵĵ��������ﵽ�����ƣ����������µĵ���
        if (totalSpawnedEnemies >= totalEnemyLimit)
        {
            return;
        }

        // �����ǰ������Ŀ�Ѵﵽ���ޣ����������µĵ���
        if (currentEnemyCount >= maxEnemies)
        {
            return;
        }

        // ���ѡ��һ������Ԥ����
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];

        // ���ѡ��һ�����ɵ�
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // ���ɵ���
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        currentEnemyCount++;
        totalSpawnedEnemies++;  // ���������ɵĵ�����Ŀ
        activeEnemies.Add(newEnemy);

        // �������������¼�
        EnemyElement enemyElement = newEnemy.GetComponent<EnemyElement>();
        if (enemyElement != null)
        {
            enemyElement.OnEnemyDeath += HandleEnemyDeath;
        }
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
            SpawnEnemy();
        }
    }

    // ����Boss
    private void SpawnBoss()
    {
        bossSpawned = true; // ���Boss�Ѿ�����

        // ��ָ���ص�����Boss
        Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        Debug.Log("Boss����!");
    }
}
