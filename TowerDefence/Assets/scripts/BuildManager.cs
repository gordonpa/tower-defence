using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("可选炮塔数据")]
    public TurretData standardTurretData;
    public TurretData missileTurretData;

    [Header("运行时")]
    public TurretData selectedTurretData;

    [Header("UI 引用")]
    public TextMeshProUGUI moneyText;
    private Animator moneyTextAnim;
    public UpgradeUI upgradeUI;

    [Header("敌人图鉴面板")]
    public GameObject enemyGalleryPanel;   // 拖到 Inspector

    private MapCube upgradeCube;

    [Header("开始界面")]
    public GameObject startPanel;     // 拖到 Inspector：初始 StartPanel
    public GameObject turretTogglePanel; // 拖到 Inspector：炮塔选择面板

    /* ========== 单例 ========== */
    private void Awake()
    {
        Instance = this;
        moneyTextAnim = moneyText.GetComponent<Animator>();

        // 1. 订阅怪物掉落金币事件
        Enemy.OnDeathBounty += ChangeMoney;
        Time.timeScale = 0f;
    }

    /* ========== 选塔回调（Toggle） ========== */
    public void OnStandardSelected(bool isOn)
    {
        if (isOn) selectedTurretData = standardTurretData;
    }

    public void OnMissileSelected(bool isOn)
    {
        if (isOn) selectedTurretData = missileTurretData;
    }

    /* ========== 金币相关 ========== */

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

    /* ========== 升级 UI ========== */
    public void ShowUpgradeUI(MapCube cube, Vector3 position, bool isDisableUpgrade)
    {
        upgradeCube = cube;
        upgradeUI.Show(position, isDisableUpgrade, cube.CurrentLevel);
    }

    public void HideUpgradeUI() => upgradeUI.Hide();

    /* ========== 按钮回调 ========== */
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
        startPanel.SetActive(false);        // 关闭自身
        turretTogglePanel.SetActive(true);  // 打开炮塔选择面板
        Time.timeScale = 1f;           // 恢复游戏
    }

    public void OnEnemyGalleryButtonClicked()
    {
        enemyGalleryPanel.SetActive(true);
        Time.timeScale = 0f;           // 暂停游戏
    }

    public void OnEnemyGalleryCloseClicked()
    {
        enemyGalleryPanel.SetActive(false);
        Time.timeScale = 1f;           // 恢复游戏
    }

    /* ========== 初始金币 ========== */
    private int money = 1250;
}