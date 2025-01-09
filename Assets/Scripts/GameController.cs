using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public enum GameStates { FreeRoam, Pause, Dialog }
    public Canvas canvas;
    private Canvas currentCanvas;
    public GameObject pauseUI;
    private bool pause;

    public bool Pause
    {
        get {return pause;}
        set {pause = value;}
    }

    public GameStates gameStates;

    public static GameController Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(canvas);
            currentCanvas = canvas;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log(currentCanvas);
            Destroy(currentCanvas);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("States Game Now:"+gameStates);
        if (gameStates == GameStates.FreeRoam)
        {
            MovementHandler.Instance.HandleUpdate();
            Time.timeScale = 1;
        }
        else if (gameStates == GameStates.Pause)
        {
            Time.timeScale = 0;
        }
        else if (gameStates == GameStates.Dialog)
        {
            Debug.Log("GameState: "+gameStates);
        }
    }

    public void TestButton()
    {
        Debug.Log("This Is Button");
    }

    public void SetPause()
    {
        pause = !pause;
        if (pause)
        {
            StartCoroutine(MusicManager.instance.ReduceBass(2000f));
            gameStates = GameStates.Pause;
            pauseUI.SetActive(true);
        }
        else
        {
            StartCoroutine(MusicManager.instance.ResetBass());
            gameStates = GameStates.FreeRoam;
            pauseUI.SetActive(false);
        }
    }
}
