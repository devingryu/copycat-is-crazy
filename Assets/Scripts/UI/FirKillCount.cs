using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirKillCount : MonoBehaviour
{
    private int killCount = Player.FirKill; // 1P의 킬 카운트
    public TMP_Text killCountText; // 킬 카운트를 출력할 TMP_Text 컴포넌트

    public void IncreaseKillCount()
    {
        UpdateKillCountText();
    }

    private void UpdateKillCountText()
    {
        // TMP_Text에 킬 카운트를 업데이트하여 출력
        killCountText.text = killCount + "Kill";
    }
}