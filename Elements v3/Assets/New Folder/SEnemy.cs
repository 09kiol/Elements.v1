using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEnemy : MonoBehaviour
{
    public GameObject prefabToSpawn; // ��Ҫ���ɵ�Ԥ����
    public int numberOfPrefabs = 5;  // Ԥ�����������
    public float spawnInterval = 1f; // ���ɼ��
    public float spawnRadius = 5f;   // ����Ԥ����İ뾶

    private Transform player;        // ��ҵ�Transform
    private int prefabsSpawned = 0;  // �����ɵ�Ԥ��������
    private float timer = 0f;        // ��ʱ��

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // �ҵ���ǩΪ"Player"����Ϸ����
    }

    void Update()
    {
        // ������Һ�Ŀ������֮��ľ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �����ҽӽ���������Ԥ����
        if (distanceToPlayer < spawnRadius && prefabsSpawned < numberOfPrefabs)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                // �������λ��
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;

                // ʵ����Ԥ����
                Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

                // ��������һ
                prefabsSpawned++;
                timer = 0f; // ���ü�ʱ��
            }
        }
    }
}