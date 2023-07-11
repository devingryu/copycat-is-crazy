using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    int winner = 0; // 1p : 1, 2p : 2
    int battleMapNumber = 0;
    int battleCount = 0;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(Instance != this)
            {
                Destroy(gameObject);
            }
        }
        battleMapNumber = SceneManager.sceneCountInBuildSettings - 3;
    }

    public void SetWinner(bool isFirstPlayerDefeated)
    {
        if(isFirstPlayerDefeated)
            winner = 2;
        else
            winner = 1;
    }

    public int GetWinner()
    {
        return winner;
    }

    public void EndGame()
    {
        SceneManager.LoadScene("ResultScene");
    }

    public void StartGame()
    {
        System.GC.Collect();
        battleCount = battleCount % battleMapNumber + 1; // 1 ~ map number
        SceneManager.LoadScene("GameScene" + battleCount);
    }
}
