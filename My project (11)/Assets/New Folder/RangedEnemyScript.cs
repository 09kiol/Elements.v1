using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyScript : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;
    public float detectionDistance = 20f; // Զ���������
    public float attackDistance = 15f; // ��������
    public float stopDistance = 10f; // ֹͣ�����ľ���
    public GameObject bulletPrefab; // �ӵ�Ԥ�Ƽ�
    public Transform firePoint; // �ӵ�����λ��
    public float bulletSpeed = 10f; // �ӵ��ٶ�
    public float attackCooldown = 2f; // ������ȴʱ��
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // ���ó�ʼĿ��Ϊ���λ��
            agent.destination = player.transform.position;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // ����Ŀ��Ϊ���λ�ã���ҿ����ƶ���
            agent.destination = player.transform.position;

            // ��������ҵľ���
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);

            // �����ҽ����ⷶΧ�ڣ���������
            if (playerDistance < detectionDistance)
            {
                // �������ڹ�����Χ�ڣ�������ֹͣ�����ڣ����������
                if (playerDistance > stopDistance && playerDistance <= attackDistance)
                {
                    agent.isStopped = false;
                }
                // ��������ֹͣ�����ڣ���ֹͣ�ƶ�
                else if (playerDistance <= stopDistance)
                {
                    agent.isStopped = true;
                    // ������
                    if (Time.time > lastAttackTime + attackCooldown)
                    {
                        ShootPlayer();
                        lastAttackTime = Time.time;
                    }
                }
                else
                {
                    agent.isStopped = true;
                }
            }
            else
            {
                // �����ҳ��������룬ֹͣ�ƶ�
                agent.isStopped = true;
            }
        }
    }

    private void ShootPlayer()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (player.transform.position - firePoint.position).normalized;
                rb.velocity = direction * bulletSpeed;
            }
        }
    }
}