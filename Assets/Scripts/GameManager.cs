using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum GameType { Normal, NoItem, TimeAttack }
    GameType gameType;
    public static GameManager Instance {  get; private set; }
    int winner = 0; // 1p : 1, 2p : 2 , draw : 3
    int battleMapNumber = 0;
    public int battleCount = 0;
    [SerializeField] FadeIn fadein;
    [SerializeField] FadeOut fadeOut;

    public int[] Kill = new int[2] { 0, 0 };
    public GameType GetGameType() => gameType;

    private void Awake()
    {
        if(Instance == null)
        {
            sfxChannels = new AudioSource[sfxChannelCount];
            Instance = this;
            for(int i = 1;i<= sfxChannelCount; ++i)
                sfxChannels[i-1] = transform.Find("sfx" + i).GetComponent<AudioSource>();
            exitPopup.gameObject.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(Instance != this)
            {
                Destroy(gameObject);
            }
        }
       
        battleMapNumber = SceneManager.sceneCountInBuildSettings - 4;
    }


    #region GameFlow

    public void SetWinner(int Winner)
    {
        winner = Winner;
    }

    public int GetWinner()
    {
        return winner;
    }

    public void EndGame()
    {
        SceneManager.LoadScene("ResultScene");
    }


    public void StartGame(int mapNumber = 0)
    { 
        //System.GC.Collect();
        if (mapNumber == 0)
        {
            battleCount = Random.Range(0, battleMapNumber) + 1;
        }
        else
            battleCount = mapNumber;


        switch(battleCount)
        {
            case 1:
                gameType = GameType.Normal;
                break;
            case 2:
                gameType = GameType.Normal;
                break;
            case 3:
                gameType = GameType.NoItem;
                break;
            case 4:
                gameType = GameType.TimeAttack;
                break;
        }

        Kill[0] = 0;
        Kill[1] = 0;
        winner = 0;
        fadeOut.FadeOUT();
        SceneManager.LoadScene("GameScene" + battleCount);
        fadein.FadeIN();
    }

    public void GoToSelectScene()
    {
        fadeOut.FadeOUT();
        SceneManager.LoadScene("GameTypeSelectScene");
        fadein.FadeIN();
    }

    public void FirKillUpdate()
    {
        Kill[0]++;
    }
    public void SecKillUpdate()
    {
        Kill[1]++;
    }
    #endregion

    #region ExitPopUp
    [SerializeField] Transform exitPopup;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(exitPopup.gameObject.activeSelf)
            {
                TurnOffExitButton();
            }
            else
            {
                Time.timeScale = 0;
                exitPopup.gameObject.SetActive(true);
            }
        }
        
    }

    public void TurnOffExitButton()
    {
        if (exitPopup.gameObject.activeSelf)
        {
            Time.timeScale = 1;
            exitPopup.gameObject.SetActive(false);
        }
    }



    public void TurnOffProgram()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
#endregion

    #region music
    public enum SFXName 
    {
        Damaged, 
        PlaceBalloon, 
        CollectItem, 
        PushBox, 
        RideSomething,
        Explode,
    }

    [SerializeField] AudioClip[] sfxClips;
    const int sfxChannelCount = 3;
    int availableSfxChannelIndex = 0;
    AudioSource[] sfxChannels;

    public void PlaySound(SFXName name)
    {

        switch(name)
        {
            case SFXName.Damaged:
                sfxChannels[availableSfxChannelIndex].clip = sfxClips[(int)SFXName.Damaged];
                sfxChannels[availableSfxChannelIndex].volume = 1;
                break;
            case SFXName.PlaceBalloon:
                sfxChannels[availableSfxChannelIndex].clip = sfxClips[(int)SFXName.PlaceBalloon];
                sfxChannels[availableSfxChannelIndex].volume = 1;
                break;
            case SFXName.CollectItem:
                sfxChannels[availableSfxChannelIndex].clip = sfxClips[(int)SFXName.CollectItem];
                sfxChannels[availableSfxChannelIndex].volume = 1;
                break;
            case SFXName.PushBox:
                sfxChannels[availableSfxChannelIndex].clip = sfxClips[(int)SFXName.PushBox];
                sfxChannels[availableSfxChannelIndex].volume = 1;
                break;
            case SFXName.RideSomething:
                sfxChannels[availableSfxChannelIndex].clip = sfxClips[(int)SFXName.RideSomething];
                sfxChannels[availableSfxChannelIndex].volume = 1;
                break;
            case SFXName.Explode:
                sfxChannels[availableSfxChannelIndex].clip = sfxClips[(int)SFXName.Explode];
                sfxChannels[availableSfxChannelIndex].volume = 0.2f;
                break;
        }

        sfxChannels[availableSfxChannelIndex].Play();
        availableSfxChannelIndex = (availableSfxChannelIndex + 1) % sfxChannelCount;
    }

    #endregion
}
