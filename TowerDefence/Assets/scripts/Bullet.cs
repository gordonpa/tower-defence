using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage = 50;       // ← 改为私有，由外部设定
    public float speed = 15f;

    private Transform target;

    private void Update()
    {
        if (target == null)
        {
            Dead();
            return;
        }

        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
            Dead();
        }
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    // 新增：让炮塔把伤害值传进来
    public void SetDamage(float dmg)
    {
        damage = Mathf.RoundToInt(dmg);
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}