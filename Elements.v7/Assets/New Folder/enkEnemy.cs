using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public GameObject bulletPrefab;  // �ӵ�Ԥ����
    public Transform firePoint;       // �����ӵ���λ��
    public Transform player;          // ��Ҷ���
    public float fireRate = 2.0f;     // ������
    public Transform[] movePoints;    // �ƶ�Ŀ���
    public float moveSpeed = 2.0f;    // �ƶ��ٶ�

    private float fireTimer = 0f;     // �����ʱ��
    private float moveTimer = 0f;     // �ƶ���ʱ��
    private float moveInterval = 5.0f; // �ƶ����
    private Transform currentTarget;  // ��ǰ�ƶ���Ŀ���

    private void Start()
    {
        MoveToRandomPosition();  // ��ʼ��λ��
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;

        // ÿ�ε��﷢����ʱ������Ҳ������ӵ�
        if (fireTimer >= fireRate)
        {
            FacePlayer();  // �������
            FireBullet();  // �����ӵ�
            fireTimer = 0f;  // ���÷����ʱ��
        }

        // ÿ�ε����ƶ����ʱѡ����Ŀ���
        if (moveTimer >= moveInterval)
        {
            MoveToRandomPosition();  // �ƶ����µ�Ŀ��λ��
            moveTimer = 0f;  // �����ƶ���ʱ��
        }

        MoveTowardsTarget();  // ÿ֡���ƶ���Ŀ���
    }

    // �����ӵ�
    private void FireBullet()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("Զ�̵��˷�����һ���ӵ���");
    }

    // �������
    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        Debug.Log("Զ�̵��˳�����ҹ�����");
    }

    // �ƶ������λ��
    private void MoveToRandomPosition()
    {
        if (movePoints == null || movePoints.Length == 0)
        {
            Debug.LogError("movePoints ����Ϊ�գ���ȷ���Ѿ�Ϊ�����Ŀ��㣡");
            return;
        }

        int randomIndex = Random.Range(0, movePoints.Length);
        currentTarget = movePoints[randomIndex];
        Debug.Log("Զ�̵���ѡ�����µ�Ŀ��λ�ã�" + currentTarget.name);
    }

    // ��Ŀ��λ�����ƶ�
    private void MoveTowardsTarget()
    {
        if (currentTarget != null)
        {
            // ���㳯��Ŀ��ķ��򣬲�ʹ��������Ŀ��
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // �ƶ���Ŀ���
            transform.position = Vector3.MoveTowards(
                transform.position,
                currentTarget.position,
                moveSpeed * Time.deltaTime
            );

            // ����Ŀ����ֹͣ�ƶ�
            if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                Debug.Log("Զ�̵��˵�����Ŀ��λ�á�");
                currentTarget = null;
            }
        }
    }
}
