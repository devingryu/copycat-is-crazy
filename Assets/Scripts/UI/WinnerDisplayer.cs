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
        if (GameManager.Instance.battleCount != 4)
        {
            if (Player.FirstPlayerDefeat && Player.SecondPlayerDefeat)
                GameManager.Instance.SetWinner(3);

            else if (Player.FirstPlayerDefeat)
                GameManager.Instance.SetWinner(2);
            else
                GameManager.Instance.SetWinner(1);

            winner = GameManager.Instance.GetWinner();
            if (winner != 3)
                whoIsWinnerText.SetText(winner + "P가 승리했습니다!");
            else
                whoIsWinnerText.SetText("양 플레이어가 비겼습니다!");

            Player.FirstPlayerDefeat = false;
            Player.SecondPlayerDefeat = false;
        }
        else
        {
            if (GameManager.Instance.Kill[0] > GameManager.Instance.Kill[1])
                GameManager.Instance.SetWinner(1);
            else if (GameManager.Instance.Kill[0] < GameManager.Instance.Kill[1])
                GameManager.Instance.SetWinner(2);
            else
                GameManager.Instance.SetWinner(3);

            winner = GameManager.Instance.GetWinner();
            if (winner != 3)
                whoIsWinnerText.SetText(winner + "P가 승리했습니다!");
            else
                whoIsWinnerText.SetText("양 플레이어가 비겼습니다!");
        }
    }
}
