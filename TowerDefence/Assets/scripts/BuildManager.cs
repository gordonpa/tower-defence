using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("��ѡ��������")]
    public TurretData standardTurretData;
    public TurretData missileTurretData;

    [Header("����ʱ")]
    public TurretData selectedTurretData;

    [Header("UI ����")]
    public TextMeshProUGUI moneyText;
    private Animator moneyTextAnim;
    public UpgradeUI upgradeUI;

    [Header("����ͼ�����")]
    public GameObject enemyGalleryPanel;   // �ϵ� Inspector

    private MapCube upgradeCube;

    [Header("��ʼ����")]
    public GameObject startPanel;     // �ϵ� Inspector����ʼ StartPanel
    public GameObject turretTogglePanel; // �ϵ� Inspector������ѡ�����

    /* ========== ���� ========== */
    private void Awake()
    {
        Instance = this;
        moneyTextAnim = moneyText.GetComponent<Animator>();

        // 1. ���Ĺ���������¼�
        Enemy.OnDeathBounty += ChangeMoney;
        Time.timeScale = 0f;
    }

    /* ========== ѡ���ص���Toggle�� ========== */
    public void OnStandardSelected(bool isOn)
    {
        if (isOn) selectedTurretData = standardTurretData;
    }

    public void OnMissileSelected(bool isOn)
    {
        if (isOn) selectedTurretData = missileTurretData;
    }

    /* ========== ������ ========== */

    private void OnEnable()
    {
        Enemy.OnDeathBounty += ChangeMoney;
    }

    private void OnDisable()
    {
        Enemy.OnDeathBounty -= ChangeMoney;
    }

    private void OnDestroy()
    {
        Enemy.OnDeathBounty -= ChangeMoney;
    }

    public bool IsEnough(int need)
    {
        if (need <= money) return true;
        MoneyFlicker();
        return false;
    }

    public void ChangeMoney(int value)
    {
        money += value;
        moneyText.text = "coin $:" + money.ToString();
    }

    public void MoneyFlicker() => moneyTextAnim.SetTrigger("Flicker");

    /* ========== ���� UI ========== */
    public void ShowUpgradeUI(MapCube cube, Vector3 position, bool isDisableUpgrade)
    {
        upgradeCube = cube;
        upgradeUI.Show(position, isDisableUpgrade, cube.CurrentLevel);
    }

    public void HideUpgradeUI() => upgradeUI.Hide();

    /* ========== ��ť�ص� ========== */
    public void OnTurretUpgrade()
    {
        upgradeCube?.OnTurretUpgrade();
        HideUpgradeUI();
    }

    public void OnTurretDestroy()
    {
        upgradeCube?.OnTurretDestroy();
        HideUpgradeUI();
    }

    public void OnStartGameButtonClicked()
    {
        startPanel.SetActive(false);        // �ر�����
        turretTogglePanel.SetActive(true);  // ������ѡ�����
        Time.timeScale = 1f;           // �ָ���Ϸ
    }

    public void OnEnemyGalleryButtonClicked()
    {
        enemyGalleryPanel.SetActive(true);
        Time.timeScale = 0f;           // ��ͣ��Ϸ
    }

    public void OnEnemyGalleryCloseClicked()
    {
        enemyGalleryPanel.SetActive(false);
        Time.timeScale = 1f;           // �ָ���Ϸ
    }

    /* ========== ��ʼ��� ========== */
    private int money = 1250;
}