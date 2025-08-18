using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage = 50;       // �� ��Ϊ˽�У����ⲿ�趨
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

    // ���������������˺�ֵ������
    public void SetDamage(float dmg)
    {
        damage = Mathf.RoundToInt(dmg);
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}