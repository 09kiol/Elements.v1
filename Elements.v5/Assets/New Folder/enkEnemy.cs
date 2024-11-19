using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public GameObject bulletPrefab;  // 子弹预制体
    public Transform firePoint;       // 发射子弹的位置
    public Transform player;          // 玩家对象
    public float fireRate = 2.0f;     // 发射间隔
    public Transform[] movePoints;    // 移动目标点
    public float moveSpeed = 2.0f;    // 移动速度

    private float fireTimer = 0f;     // 发射计时器
    private float moveTimer = 0f;     // 移动计时器
    private float moveInterval = 5.0f; // 移动间隔
    private Transform currentTarget;  // 当前移动的目标点

    private void Start()
    {
        MoveToRandomPosition();  // 初始化位置
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;

        // 每次到达发射间隔时朝向玩家并发射子弹
        if (fireTimer >= fireRate)
        {
            FacePlayer();  // 朝向玩家
            FireBullet();  // 发射子弹
            fireTimer = 0f;  // 重置发射计时器
        }

        // 每次到达移动间隔时选择新目标点
        if (moveTimer >= moveInterval)
        {
            MoveToRandomPosition();  // 移动到新的目标位置
            moveTimer = 0f;  // 重置移动计时器
        }

        MoveTowardsTarget();  // 每帧逐渐移动到目标点
    }

    // 发射子弹
    private void FireBullet()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("远程敌人发射了一颗子弹！");
    }

    // 朝向玩家
    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        Debug.Log("远程敌人朝向玩家攻击。");
    }

    // 移动到随机位置
    private void MoveToRandomPosition()
    {
        if (movePoints == null || movePoints.Length == 0)
        {
            Debug.LogError("movePoints 数组为空，请确保已经为其分配目标点！");
            return;
        }

        int randomIndex = Random.Range(0, movePoints.Length);
        currentTarget = movePoints[randomIndex];
        Debug.Log("远程敌人选择了新的目标位置：" + currentTarget.name);
    }

    // 朝目标位置逐渐移动
    private void MoveTowardsTarget()
    {
        if (currentTarget != null)
        {
            // 计算朝向目标的方向，并使敌人面向目标
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // 移动到目标点
            transform.position = Vector3.MoveTowards(
                transform.position,
                currentTarget.position,
                moveSpeed * Time.deltaTime
            );

            // 到达目标点后停止移动
            if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                Debug.Log("远程敌人到达了目标位置。");
                currentTarget = null;
            }
        }
    }
}
