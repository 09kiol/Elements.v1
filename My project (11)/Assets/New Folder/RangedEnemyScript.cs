using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyScript : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;
    public float detectionDistance = 20f; // 远距离检测距离
    public float attackDistance = 15f; // 攻击距离
    public float stopDistance = 10f; // 停止靠近的距离
    public GameObject bulletPrefab; // 子弹预制件
    public Transform firePoint; // 子弹发射位置
    public float bulletSpeed = 10f; // 子弹速度
    public float attackCooldown = 2f; // 攻击冷却时间
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 设置初始目标为玩家位置
            agent.destination = player.transform.position;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // 更新目标为玩家位置（玩家可能移动）
            agent.destination = player.transform.position;

            // 计算与玩家的距离
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);

            // 如果玩家进入检测范围内，则跟踪玩家
            if (playerDistance < detectionDistance)
            {
                // 如果玩家在攻击范围内，但不在停止距离内，则继续跟踪
                if (playerDistance > stopDistance && playerDistance <= attackDistance)
                {
                    agent.isStopped = false;
                }
                // 如果玩家在停止距离内，则停止移动
                else if (playerDistance <= stopDistance)
                {
                    agent.isStopped = true;
                    // 射击玩家
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
                // 如果玩家超出检测距离，停止移动
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