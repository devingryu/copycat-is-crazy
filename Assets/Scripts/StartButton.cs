using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void StartBtn()
    {
        //SceneManager.LoadScene("GameScene1");
        GameManager.Instance.StartGame();
    }
}
