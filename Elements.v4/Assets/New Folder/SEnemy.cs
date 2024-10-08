using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEnemy : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // ���ֵ���Ԥ����
    public int maxEnemies = 40;       // ��ǰͬʱ���ڵĵ�������
    public int totalEnemyLimit = 60;  // �������ɵ�����������
    private int currentEnemyCount = 0;    // ��ǰ��ĵ�������
    private int totalSpawnedEnemies = 0;  // �Ѿ����ɵĵ�������
    public Transform[] spawnPoints;   // ˢ�µĵص�

    private List<GameObject> activeEnemies = new List<GameObject>(); // ���ĵ����б�

    private void Start()
    {
        // ��ʼʱ���ɳ�ʼ�����ĵ���
        for (int i = 0; i < maxEnemies; i++)
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

        if (currentEnemyCount >= maxEnemies)
        {
            return; // �ﵽ��ǰͬʱ���ڵĵ������ޣ���������
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

        // �Ƴ��Ѿ������ĵ���
        activeEnemies.RemoveAll(enemy => enemy == null);

        // �����û�дﵽ�������ɵ������ƣ��ҵ�ǰ��������û�г������ޣ��������µĵ���
        if (currentEnemyCount < maxEnemies && totalSpawnedEnemies < totalEnemyLimit)
        {
            SpawnEnemy();
        }
    }
}
