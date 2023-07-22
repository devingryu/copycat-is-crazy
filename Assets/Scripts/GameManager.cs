using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    int winner = 0; // 1p : 1, 2p : 2 , draw : 3
    int battleMapNumber = 0;
    public static int battleCount = 0;
    public FadeIn Fadein;
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
        battleMapNumber = SceneManager.sceneCountInBuildSettings - 3;
        
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

    public void StartGame()
    {
        System.GC.Collect();
        battleCount = battleCount % battleMapNumber + 1; // 1 ~ map number
        SceneManager.LoadScene("GameScene" + battleCount);
        
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
                Time.timeScale = 1;
                exitPopup.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                exitPopup.gameObject.SetActive(true);
            }
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
                break;
            case SFXName.PlaceBalloon:
                sfxChannels[availableSfxChannelIndex].clip = sfxClips[(int)SFXName.PlaceBalloon];
                break;
            case SFXName.CollectItem:
                sfxChannels[availableSfxChannelIndex].clip = sfxClips[(int)SFXName.CollectItem];
                break;
            case SFXName.PushBox:
                sfxChannels[availableSfxChannelIndex].clip = sfxClips[(int)SFXName.PushBox];
                break;
            case SFXName.RideSomething:
                sfxChannels[availableSfxChannelIndex].clip = sfxClips[(int)SFXName.RideSomething];
                break;
            case SFXName.Explode:
                sfxChannels[availableSfxChannelIndex].clip = sfxClips[(int)SFXName.Explode];
                break;
        }

        sfxChannels[availableSfxChannelIndex].Play();
        availableSfxChannelIndex = (availableSfxChannelIndex + 1) % sfxChannelCount;
    }

    #endregion
}
