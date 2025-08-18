using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    private Animator anim;
    public Button upgradeButton;
    public TextMeshProUGUI upgradeButtonText;   // 把按钮上的 TMP 拖进来

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (upgradeButtonText == null)
            upgradeButtonText = upgradeButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    /* 新版 Show：带等级参数 */
    public void Show(Vector3 position, bool isDisableUpgrade, MapCube.TurretLevel level)
    {
        // 如果面板已在此位置，则关闭
        if (transform.localScale != Vector3.zero && transform.position == position)
        {
            Hide();
            return;
        }

        /* 根据等级设置按钮文字 & 禁用状态 */
        upgradeButtonText.text = level switch
        {
            MapCube.TurretLevel.None => "升级",
            MapCube.TurretLevel.Lv1 => "升级",
            MapCube.TurretLevel.Lv2 => "升级",
            MapCube.TurretLevel.Lv3 => "Max",
            _ => "Upgrade"
        };

        upgradeButton.interactable = !isDisableUpgrade;

        transform.position = position;
        anim.SetBool("isShow", true);
    }

    public void Hide()
    {
        anim.SetBool("isShow", false);
    }

    /* 按钮回调 */
    public void OnUpgradeButtonClick() => BuildManager.Instance.OnTurretUpgrade();
    public void OnDestroyButtonClick() => BuildManager.Instance.OnTurretDestroy();
}