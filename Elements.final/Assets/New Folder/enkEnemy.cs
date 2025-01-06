using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float chaseDistance = 10f;
    public float stopDistance = 5f;
    public float fireRate = 2.0f;

    private float fireTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("δ�ҵ���ǩΪ 'Player' �Ľ�ɫ��");
        }
    }

    void Update()
    {
        if (player == null) return;

        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        if (playerDistance <= chaseDistance)
        {
            agent.destination = player.transform.position;

            if (playerDistance <= stopDistance)
            {
                agent.isStopped = true;
                FacePlayer();
                ShootAtPlayer();
            }
            else
            {
                agent.isStopped = false;
                FacePlayer();
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }

    private void ShootAtPlayer()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Debug.Log("Զ�̵��˷�����һ���ӵ���");
            fireTimer = 0f;
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}