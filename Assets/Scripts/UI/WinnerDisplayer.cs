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
        if (GameManager.battleCount != 4)
        {
            if (Player.FirstPlayerDefeat && Player.SecondPlayerDefeat)
                GameManager.Instance.SetWinner(3);

            else if (Player.FirstPlayerDefeat)
                GameManager.Instance.SetWinner(2);
            else
                GameManager.Instance.SetWinner(1);

            winner = GameManager.Instance.GetWinner();
            if (winner != 3)
                whoIsWinnerText.SetText(winner + "P�� �¸��߽��ϴ�!");
            else
                whoIsWinnerText.SetText("�� �÷��̾ �����ϴ�!");

            Player.FirstPlayerDefeat = false;
            Player.SecondPlayerDefeat = false;
        }
        else
        {
            if (Player.FirKill > Player.SecKill)
                GameManager.Instance.SetWinner(1);
            else if (Player.FirKill < Player.SecKill)
                GameManager.Instance.SetWinner(2);
            else
                GameManager.Instance.SetWinner(3);

            winner = GameManager.Instance.GetWinner();
            if (winner != 3)
                whoIsWinnerText.SetText(winner + "P�� �¸��߽��ϴ�!");
            else
                whoIsWinnerText.SetText("�� �÷��̾ �����ϴ�!");
        }
    }
}