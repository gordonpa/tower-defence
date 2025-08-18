using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    /* ---------------- ԭ���ֶ� ---------------- */
    private int pointIndex = 0;
    private Vector3 targetPosition = Vector3.zero;
    public float speed = 4;
    public float hp = 110;
    private float maxHP = 0;
    public GameObject explosionPrefab;
    private Slider hpSlider;
    [SerializeField] private float baseSpeed;      // ԭʼ�ٶ�
    private float slowFactor = 1f;                 // 1 ��ʾ�޼���

    /* ---------------- �������� ---------------- */
    public int bounty = 20;                   // ���ܻ�õĽ��
    public static event Action<int> OnDeathBounty;  // ��̬�¼�

    /* ---------------- ԭ���߼� ---------------- */
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
        float currentSpeed = baseSpeed * slowFactor;   // ��ȷӦ�ü���
        transform.Translate(dir * currentSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            MoveNextPoint();
    }

    private void MoveNextPoint()
    {
        pointIndex++;
        if (pointIndex >= Waypoints.Instance.GetLength())
        {
            ReachDestination();   // �� Die() ����Ϊ�����廯�ĺ���
            return;
        }

        targetPosition = Waypoints.Instance.GetWaypoint(pointIndex);
        Vector3 lookDir = (targetPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(-lookDir, Vector3.up);
    }

    /* ---------------- �����򵽴��յ� ---------------- */
    void ReachDestination()
    {
        // �����յ㲻�ƽ�ң�ֱ������
        Destroy(gameObject);
        EnemySpawner.Instance.DecreaseEnemyCount();
    }

    void Die()          // ��������������
    {
        OnDeathBounty?.Invoke(bounty);  // ��Ǯ
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