using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    /* ---------------- 原有字段 ---------------- */
    private int pointIndex = 0;
    private Vector3 targetPosition = Vector3.zero;
    public float speed = 4;
    public float hp = 110;
    private float maxHP = 0;
    public GameObject explosionPrefab;
    private Slider hpSlider;
    [SerializeField] private float baseSpeed;      // 原始速度
    private float slowFactor = 1f;                 // 1 表示无减速

    /* ---------------- 新增掉落 ---------------- */
    public int bounty = 20;                   // 击败获得的金币
    public static event Action<int> OnDeathBounty;  // 静态事件

    /* ---------------- 原有逻辑 ---------------- */
    void Start()
    {
        baseSpeed = speed;
        targetPosition = Waypoints.Instance.GetWaypoint(pointIndex);
        hpSlider = transform.Find("Canvas/HPSlider").GetComponent<Slider>();
        hpSlider.value = 1;
        maxHP = hp;
    }

    void Update()
    {
        Vector3 dir = (targetPosition - transform.position).normalized;
        float currentSpeed = baseSpeed * slowFactor;   // 正确应用减速
        transform.Translate(dir * currentSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            MoveNextPoint();
    }

    private void MoveNextPoint()
    {
        pointIndex++;
        if (pointIndex >= Waypoints.Instance.GetLength())
        {
            ReachDestination();   // 把 Die() 改名为更语义化的函数
            return;
        }

        targetPosition = Waypoints.Instance.GetWaypoint(pointIndex);
        Vector3 lookDir = (targetPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(-lookDir, Vector3.up);
    }

    /* ---------------- 死亡或到达终点 ---------------- */
    void ReachDestination()
    {
        // 到达终点不计金币，直接销毁
        Destroy(gameObject);
        EnemySpawner.Instance.DecreaseEnemyCount();
    }

    void Die()          // 真正“被打死”
    {
        OnDeathBounty?.Invoke(bounty);  // 发钱
        EnemySpawner.Instance.DecreaseEnemyCount();

        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(go, 1);

        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        hpSlider.value = hp / maxHP;

        if (hp <= 0)
            Die();
    }

    public void AddSlow(float ratio) => slowFactor = Mathf.Max(0f, slowFactor - ratio);
    public void RemoveSlow(float ratio) => slowFactor = Mathf.Min(1f, slowFactor + ratio);
}