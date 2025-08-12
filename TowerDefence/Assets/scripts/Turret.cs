using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();

    public GameObject bulletPrefab;
    public Transform bulletPosition;

    public float attackRate = 0.1f;
    private float nextAttackTime;

    private Transform head;

    private void Start()
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

    private void Attack()
    {
        if (enemyList == null || enemyList.Count == 0) return;

        if (Time.time > nextAttackTime)
        {
            Transform target = GetTarget();
            if (target != null)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
                go.GetComponent<Bullet>().SetTarget(target);
                nextAttackTime = Time.time + attackRate;
            }
        }
    }

    public Transform GetTarget()
    {
        List<int> indexList = new List<int>();

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null )   
                indexList.Add(i);
        }

        //这里通常会继续遍历 indexList 做清理，例如：
        for (int j = indexList.Count - 1; j >= 0; j--)
            enemyList.RemoveAt(indexList[j]);

        if (enemyList != null && enemyList.Count != 0)
        {
           return enemyList[0].transform; 
        }
        return null; 
    }

    private void DirectionControl()
    {
        if (enemyList == null || enemyList.Count == 0 || head == null) return;

        Transform target = GetTarget();
        if (target == null) return;

        head.LookAt(target.position);
    }
}
