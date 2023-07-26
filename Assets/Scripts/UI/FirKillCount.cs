using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirKillCount : MonoBehaviour
{
    [SerializeField]
    TMP_Text KillCount;
    void Update()
    {
        KillCount.text = GameManager.Instance.Kill[0] + " Kill";
    }
}
