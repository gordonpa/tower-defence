// LaserTurret.cs
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Turret
{
    [Header("�������")]
    public float damagePerSecond = 60f;          // ÿ���˺�
    public float slowToRatio = 0.3f;             // ����ʱֱ�Ӽ����ñ�����0~1��

    [Header("��Ч����")]
    public LineRenderer lineRenderer;
    public Transform laserStartPoint;
    public GameObject laserEffectGO;             // ���ڵ������ϵ��ܻ���Ч

    /* ---------- ˽�л��� ---------- */
    private Enemy lastTarget;                    // ��һ֡���䵽�ĵ��ˣ����ڸ�ԭ�ٶ�

    /* ================================================================ */

    protected override void Attack()
    {
        Transform targetTrans = GetTarget();
        Enemy targetEnemy = targetTrans ? targetTrans.GetComponent<Enemy>() : null;

        if (targetEnemy == null)
        {
            ToggleLaser(false);
            RestoreSpeedIfNeeded();
            return;
        }

        ToggleLaser(true);

        // 1. �˺�
        targetEnemy.TakeDamage(damagePerSecond * Time.deltaTime);

        // 2. ���٣�����һ������õ���ʱһ��������
        if (targetEnemy != lastTarget)
        {
            RestoreSpeedIfNeeded();      // ��ԭ��һ���˵��ٶ�
            lastTarget = targetEnemy;    // ��¼��Ŀ��
            lastTarget.AddSlow(1f - slowToRatio); // ֱ�Ӽ��� slowToRatio
        }

        // 3. ���� & ��Чλ��
        DrawLaser(targetEnemy.transform.position);
    }

    /// <summary>
    /// �����һ֡�м���Ŀ�꣬��ԭ���ٶ�
    /// </summary>
    private void RestoreSpeedIfNeeded()
    {
        if (lastTarget != null)
        {
            lastTarget.RemoveSlow(1f - slowToRatio);
            lastTarget = null;
        }
    }

    /// <summary>
    /// ���ؼ������
    /// </summary>
    private void ToggleLaser(bool on)
    {
        lineRenderer.enabled = on;
        laserEffectGO.SetActive(on);
    }

    private void DrawLaser(Vector3 hitPoint)
    {
        // ������
        lineRenderer.SetPosition(0, laserStartPoint.position);
        lineRenderer.SetPosition(1, hitPoint);

        // �ܻ���Ч
        laserEffectGO.transform.position = hitPoint;
        // �ù���泯���������ÿ���
        Vector3 lookPos = laserStartPoint.position;
        lookPos.y = hitPoint.y;
        laserEffectGO.transform.LookAt(lookPos);
    }
}