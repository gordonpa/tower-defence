using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TurretData
{
    [Header("1 级炮塔")]
    public GameObject turretPrefab;   // 初始模型
    public int cost;                  // 建造费用

    [Header("2 级炮塔")]
    public GameObject turretUpgradedPrefab; // 第一次升级模型
    public int costUpgraded;                // 第一次升级费用

    [Header("3 级炮塔")]
    public GameObject turretFinalPrefab;   // 第二次升级模型
    public int costFinal;                  // 第二次升级费用

    public TurretType type;
}

public enum TurretType
{
    StandardTurret,
    MissileTurret
}