using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private int pointIndex = 0;

    private Vector3 targetPosition = Vector3.zero;

    public float speed = 4;

    public float hp = 110;
    private float maxHP = 0;
    public GameObject explosionPrefab;

    private Slider hpSlider;


    void Start()
    {
        targetPosition = Waypoints.Instance.GetWaypoint(pointIndex);
        hpSlider = transform.Find("Canvas/HPSlider").GetComponent<Slider>();
        hpSlider.value = 1;
        maxHP = hp;
    }
    // Start is called before the first frame update
    void Update()
    {
        Vector3 dir = (targetPosition - transform.position).normalized;

        // 在世界坐标系里沿 dir 直线前进
        transform.Translate(dir * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            MoveNextPoint();
    }

    private void MoveNextPoint()
    {
        pointIndex++;
        if (pointIndex >= Waypoints.Instance.GetLength())
        {
            Die();
            return;
        }

        targetPosition = Waypoints.Instance.GetWaypoint(pointIndex);

        // 把 forward 反一下即可
        Vector3 lookDir = (targetPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(-lookDir, Vector3.up);
    }

    void Die()
    {
        Destroy(gameObject);
        EnemySpawner.Instance.DecreaseEnemyCount();
        GameObject go =GameObject.Instantiate(explosionPrefab,transform.position,Quaternion.identity);
        Destroy(go,1);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        hpSlider.value = hp / maxHP;

        if (hp <= 0)
            Die();
    }

}
