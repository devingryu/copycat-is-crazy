using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToButton : MonoBehaviour
{
    public void HowToBtn()
    {
        SceneManager.LoadScene("HowToScene");
    }
}
