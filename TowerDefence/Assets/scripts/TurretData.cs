using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TurretData
{
    [Header("1 ������")]
    public GameObject turretPrefab;   // ��ʼģ��
    public int cost;                  // �������

    [Header("2 ������")]
    public GameObject turretUpgradedPrefab; // ��һ������ģ��
    public int costUpgraded;                // ��һ����������

    [Header("3 ������")]
    public GameObject turretFinalPrefab;   // �ڶ�������ģ��
    public int costFinal;                  // �ڶ�����������

    public TurretType type;
}

public enum TurretType
{
    StandardTurret,
    MissileTurret
}