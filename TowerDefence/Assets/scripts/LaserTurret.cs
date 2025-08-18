// LaserTurret.cs
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Turret
{
    [Header("激光参数")]
    public float damagePerSecond = 60f;          // 每秒伤害
    public float slowToRatio = 0.3f;             // 照射时直接减到该比例（0~1）

    [Header("特效引用")]
    public LineRenderer lineRenderer;
    public Transform laserStartPoint;
    public GameObject laserEffectGO;             // 放在敌人身上的受击特效

    /* ---------- 私有缓存 ---------- */
    private Enemy lastTarget;                    // 上一帧照射到的敌人，用于复原速度

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

        // 1. 伤害
        targetEnemy.TakeDamage(damagePerSecond * Time.deltaTime);

        // 2. 减速：仅第一次照射该敌人时一次性设置
        if (targetEnemy != lastTarget)
        {
            RestoreSpeedIfNeeded();      // 复原上一个人的速度
            lastTarget = targetEnemy;    // 记录新目标
            lastTarget.AddSlow(1f - slowToRatio); // 直接减到 slowToRatio
        }

        // 3. 画线 & 特效位置
        DrawLaser(targetEnemy.transform.position);
    }

    /// <summary>
    /// 如果上一帧有减速目标，则复原其速度
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
    /// 开关激光表现
    /// </summary>
    private void ToggleLaser(bool on)
    {
        lineRenderer.enabled = on;
        laserEffectGO.SetActive(on);
    }

    private void DrawLaser(Vector3 hitPoint)
    {
        // 激光线
        lineRenderer.SetPosition(0, laserStartPoint.position);
        lineRenderer.SetPosition(1, hitPoint);

        // 受击特效
        laserEffectGO.transform.position = hitPoint;
        // 让光斑面朝炮塔（更好看）
        Vector3 lookPos = laserStartPoint.position;
        lookPos.y = hitPoint.y;
        laserEffectGO.transform.LookAt(lookPos);
    }
}