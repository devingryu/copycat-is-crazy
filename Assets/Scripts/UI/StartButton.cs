using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] int mapNumber = 0;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartBtn);
    }

    void StartBtn()
    {
        if(mapNumber == -1)
        {
            //-1 : select scene
            GameManager.Instance.GoToSelectScene();
            return;
        }

        GameManager.Instance.StartGame(mapNumber);
    }

    public void SetMapNumber(int mapNumber)
    {
        this.mapNumber  = mapNumber;
    }
}