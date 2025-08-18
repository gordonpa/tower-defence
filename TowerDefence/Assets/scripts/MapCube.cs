using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour
{
    /* ========== �ȼ�ϵͳ ========== */
    public enum TurretLevel { None = 0, Lv1 = 1, Lv2 = 2, Lv3 = 3 }
    private TurretLevel currentLevel = TurretLevel.None;

    /* ========== ������� ========== */
    private GameObject turretGO;
    private TurretData turretData;

    /* ========== ���� ========== */
    public GameObject buildEffect;
    private Color normalColor;

    /* ========== Unity �ص� ========== */
    private void Awake()
    {
        normalColor = GetComponent<MeshRenderer>().material.color;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (turretGO != null)
        {
            // ����Ѿ��� Lv3���ͽ���������ť
            bool disableUpgrade = (currentLevel == TurretLevel.Lv3);
            BuildManager.Instance.ShowUpgradeUI(this, transform.position, disableUpgrade);
        }
        else
        {
            BuildTurret();
        }
    }

    private void OnMouseEnter()
    {
        if (turretGO == null && !EventSystem.current.IsPointerOverGameObject())
            GetComponent<MeshRenderer>().material.color = normalColor * 1.8f;
    }

    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.color = normalColor;
    }

    /* ========== ���� ========== */
    private void BuildTurret()
    {
        turretData = BuildManager.Instance.selectedTurretData;
        if (turretData == null || turretData.turretPrefab == null) return;

        int cost = turretData.cost;
        if (!BuildManager.Instance.IsEnough(cost)) return;

        BuildManager.Instance.ChangeMoney(-cost);

        turretGO = InstantiateTurret(turretData.turretPrefab);
        currentLevel = TurretLevel.Lv1;   // �ս����� 1 ��
    }

    /* ========== ���� ========== */
    public void OnTurretUpgrade()
    {
        if (turretData == null) return;

        if (currentLevel == TurretLevel.Lv1 &&
            BuildManager.Instance.IsEnough(turretData.costUpgraded))
        {
            BuildManager.Instance.ChangeMoney(-turretData.costUpgraded);
            Destroy(turretGO);
            turretGO = InstantiateTurret(turretData.turretUpgradedPrefab);
            currentLevel = TurretLevel.Lv2;
        }
        else if (currentLevel == TurretLevel.Lv2 &&
                 BuildManager.Instance.IsEnough(turretData.costFinal))
        {
            BuildManager.Instance.ChangeMoney(-turretData.costFinal);
            Destroy(turretGO);
            turretGO = InstantiateTurret(turretData.turretFinalPrefab);
            currentLevel = TurretLevel.Lv3;
        }
    }

    /* ========== ��� ========== */
    public void OnTurretDestroy()
    {
        Destroy(turretGO);
        turretData = null;
        turretGO = null;
        currentLevel = TurretLevel.None;

        GameObject effect = Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);
    }

    /* ========== ���� ========== */
    private GameObject InstantiateTurret(GameObject prefab)
    {
        GameObject go = Instantiate(prefab, transform.position + new Vector3(0,0.6f,0) , Quaternion.identity);
        GameObject effect = Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);
        return go;
    }

    /* ========== �����ṩֻ������ ========== */
    public TurretLevel CurrentLevel => currentLevel;
}