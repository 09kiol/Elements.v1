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

            // �������С���趨��׷�����룬��׷�����
            if (playerDistance < chaseDistance)
            {
                // ���ݾ�������Ƿ�ֹͣ�ƶ�
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
                // ����׷�������ֹͣ�ƶ�
                agent.isStopped = true;
            }
        }
    }
}
