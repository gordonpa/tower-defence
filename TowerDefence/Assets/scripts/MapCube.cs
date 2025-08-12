using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour
{
    private GameObject turretGO;
    private TurretData turretData;

    public GameObject buildEffect;
    private Color nomalColor;


    private void Awake()
    {
        nomalColor= GetComponent<MeshRenderer>().material.color; 
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true) return;
        TurretData selectedTD = BuildManager.Instance.selectedTurretData;
        if (selectedTD == null || selectedTD.turretPrefab == null) return;

        if (turretGO != null) return;
        BuildTurret(selectedTD);
    }

    private void BuildTurret(TurretData _turretData)
    {
        if (BuildManager.Instance.IsEnough(_turretData.cost) == false)
        {
            return;
        }
        BuildManager.Instance.ChangeMoney(-_turretData.cost);
        turretData = _turretData;
        turretGO = GameObject.Instantiate(_turretData.turretPrefab, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
        GameObject go = GameObject.Instantiate(buildEffect, transform.position + new Vector3(0, 2, 0), Quaternion.Euler(-90, 0, 0));
        Destroy(go, 2);
    }

    private void OnMouseEnter()
    {
        if (turretGO == null && EventSystem.current.IsPointerOverGameObject() == false)
            GetComponent<MeshRenderer>().material.color = nomalColor*1.8f;
    }

    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.color = nomalColor;
    }
}