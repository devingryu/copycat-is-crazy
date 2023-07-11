using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI whoIsWinnerText;
    int winner = 0;

    private void Start()
    {
        winner = GameManager.Instance.GetWinner();
        whoIsWinnerText.SetText(winner + "P�� �¸��߽��ϴ�!");
    }
}
