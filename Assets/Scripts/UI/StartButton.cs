using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    FadeOut FadeOut;

    public void StartBtn()
    {
        StartCoroutine(DelayedStartGame());
    }

    IEnumerator DelayedStartGame()
    {
        FadeOut?.FadeOUT();
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.StartGame();
    }
}