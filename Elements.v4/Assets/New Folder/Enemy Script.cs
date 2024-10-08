using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;
    public float chaseDistance = 10f;
    public float stopDistance = 3f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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

            // 如果距离小于设定的追击距离，则追击玩家
            if (playerDistance < chaseDistance)
            {
                // 根据距离决定是否停止移动
                if (playerDistance < stopDistance)
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                }
            }
            else
            {
                // 超出追击距离后停止移动
                agent.isStopped = true;
            }
        }
    }
}
