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
        agent.updateUpAxis = false;   

        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            agent.destination = player.transform.position;
        }
    }

    void Update()
    {
        if (player != null)
        {
            agent.destination = player.transform.position;

            float playerDistance = Vector3.Distance(transform.position, player.transform.position);

            if (playerDistance < chaseDistance)
            {
                agent.isStopped = playerDistance < stopDistance;
            }
            else
            {
                agent.isStopped = true;
            }
        }
    }
}