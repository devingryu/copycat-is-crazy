using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirKillCount : MonoBehaviour
{
    private int killCount = Player.FirKill; // 1P�� ų ī��Ʈ
    public TMP_Text killCountText; // ų ī��Ʈ�� ����� TMP_Text ������Ʈ

    public void IncreaseKillCount()
    {
        UpdateKillCountText();
    }

    private void UpdateKillCountText()
    {
        // TMP_Text�� ų ī��Ʈ�� ������Ʈ�Ͽ� ���
        killCountText.text = killCount + "Kill";
    }
}