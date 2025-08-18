// Turret.cs
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();

    public GameObject bulletPrefab;
    public Transform bulletPosition;

    [Header("射速与伤害")]
    public float attackRate = 0.1f;
    public float damagePerShot = 50f;          // ← 新增：每发子弹伤害

    private float nextAttackTime;
    private Transform head;

    protected virtual void Start()
    {
        head = transform.Find("Head");
        if (head == null)
            Debug.LogError("炮塔找不到名为 'Head' 的子物体！", this);
    }

    private void Update()
    {
        Attack();
        DirectionControl();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            enemyList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
            enemyList.Remove(other.gameObject);
    }

    protected virtual void Attack()
    {
        if (enemyList == null || enemyList.Count == 0) return;

        if (Time.time > nextAttackTime)
        {
            Transform target = GetTarget();
            if (target != null)
            {
                GameObject go = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
                Bullet bullet = go.GetComponent<Bullet>();
                bullet.SetTarget(target);
                bullet.SetDamage(damagePerShot);   // ← 把伤害传给子弹
                nextAttackTime = Time.time + attackRate;
            }
        }
    }

    public Transform GetTarget()
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null)
                indexList.Add(i);
        }
        for (int j = indexList.Count - 1; j >= 0; j--)
            enemyList.RemoveAt(indexList[j]);

        return (enemyList != null && enemyList.Count > 0) ? enemyList[0].transform : null;
    }

    private void DirectionControl()
    {
        if (enemyList == null || enemyList.Count == 0 || head == null) return;

        Transform target = GetTarget();
        if (target == null) return;

        Vector3 aimPos = target.position;
        aimPos.y = head.position.y;
        head.LookAt(aimPos);
    }
}