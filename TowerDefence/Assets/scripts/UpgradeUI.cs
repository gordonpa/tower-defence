using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    private Animator anim;
    public Button upgradeButton;
    public TextMeshProUGUI upgradeButtonText;   // �Ѱ�ť�ϵ� TMP �Ͻ���

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (upgradeButtonText == null)
            upgradeButtonText = upgradeButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    /* �°� Show�����ȼ����� */
    public void Show(Vector3 position, bool isDisableUpgrade, MapCube.TurretLevel level)
    {
        // ���������ڴ�λ�ã���ر�
        if (transform.localScale != Vector3.zero && transform.position == position)
        {
            Hide();
            return;
        }

        /* ���ݵȼ����ð�ť���� & ����״̬ */
        upgradeButtonText.text = level switch
        {
            MapCube.TurretLevel.None => "����",
            MapCube.TurretLevel.Lv1 => "����",
            MapCube.TurretLevel.Lv2 => "����",
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

    /* ��ť�ص� */
    public void OnUpgradeButtonClick() => BuildManager.Instance.OnTurretUpgrade();
    public void OnDestroyButtonClick() => BuildManager.Instance.OnTurretDestroy();
}