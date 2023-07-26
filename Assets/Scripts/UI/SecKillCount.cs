using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecKillCount : MonoBehaviour
{
    [SerializeField]
    TMP_Text KillCount;
    void Update()
    {
        KillCount.text = GameManager.Instance.Kill[1] + " Kill";
    }
}
