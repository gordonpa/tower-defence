using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    public TurretData standardTurretData;
    public TurretData missileTurretData;

    public TurretData selectedTurretData;

    public TextMeshProUGUI moneyText;
    private Animator moneyTextAnim;
    private int money = 1000;

    private void Awake()
    {
        Instance = this;
        moneyTextAnim = moneyText.GetComponent<Animator>();
    }

    public void OnStandardSelected(bool isOn)
    {
        if (isOn)
            selectedTurretData = standardTurretData;
    }

    public void OnMissileSelected(bool isOn)
    {
        if (isOn)
            selectedTurretData = missileTurretData;
    }

    public bool IsEnough(int need)
    {
        if (need <= money)
        {
            return true;
        }
        else
        {
            MoneyFlicker();
            return false;
        }
            
    }

    public void ChangeMoney(int value)
    {
        this.money += value;
        moneyText.text = "coin $:"+ money.ToString();
    }

    public void MoneyFlicker()
    {
        moneyTextAnim.SetTrigger("Flicker");
    }
}
