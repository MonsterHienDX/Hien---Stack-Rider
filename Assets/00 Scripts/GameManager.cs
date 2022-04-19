using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerBall playerBall;
    public TutorialManager tutorialManager;
    public bool isLose;
    public static GameManager instance;
    public bool isGamePlaying = true;
    public List<GameObject> levelPrefabs;
    [SerializeField] Transform levelRoot;
    private GameObject currentLevel;
    [SerializeField] private EndGamePanelManager endGamePanelManager;
    [SerializeField] private FloatingTextManager floatingTextManager;

    public int levelNumber;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isLose = false;
        SetFPS(Constant.FPS);
        levelNumber = GetLevelNumber();
        LoadLevel();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isGamePlaying)
        {
            isGamePlaying = true;
            tutorialManager.EnableTutorial(false);
            playerBall.StartPlay();
        }
    }

    public void BallMoveLeftRight(float xPos)
    {
        playerBall.DragLeftRight(-xPos);
    }

    public void LoadLevel()
    {
        tutorialManager.EnableTutorial(true);
        isLose = false;
        isGamePlaying = false;
        if (levelRoot.childCount > 0)
            Destroy(levelRoot.GetChild(0).gameObject);


        if (levelNumber < levelPrefabs.Count)
            currentLevel = Instantiate<GameObject>(levelPrefabs[levelNumber], levelRoot);
        else
        {
            currentLevel = Instantiate<GameObject>(levelPrefabs[UnityEngine.Random.Range(0, levelPrefabs.Count)], levelRoot);
        }

        currentLevel.transform.SetParent(levelRoot);

        LevelInfo levelInfo = currentLevel.GetComponent<LevelInfo>();

        playerBall.Init(levelInfo);

        EventDispatcher.Instance.PostEvent(EventID.LoadLevel, levelNumber);

        endGamePanelManager.HidePopup();



    }

    private int GetLevelNumber()
    {
        if (PlayerPrefs.HasKey(Constant.KEY_LEVEL))
        {
            return PlayerPrefs.GetInt(Constant.KEY_LEVEL, 0);
        }
        else
        {
            PlayerPrefs.SetInt(Constant.KEY_LEVEL, 1);
            return 1;
        }
    }

    // Set level number
    public void SetLevelNumber(int level)
    {
        PlayerPrefs.SetInt(Constant.KEY_LEVEL, level);
    }

    public void EndLevel(bool isWin)
    {
        EventDispatcher.Instance.PostEvent(EventID.EndLevel, isWin);
        playerBall.StopMove();
        currentLevel = null;

        if (isWin)
        {

            Vibrator.Vibrate(Constant.STRONG_VIBRATE);
            EventDispatcher.Instance.PostEvent(EventID.ChangeCharacterState, Constant.WIN);
            playerBall.DestroyBallWhenWin();
        }
        else
        {
            Vibrator.Vibrate(Constant.WEAK_VIBRATE);
            EventDispatcher.Instance.PostEvent(EventID.ChangeCharacterState, Constant.LOSE);
        }
        // _endGamePanelManager.ShowPopup(isWin);
    }

    public void IncreaseCoin()
    {
        if (PlayerPrefs.HasKey(Constant.KEY_SAVE_COIN))
            PlayerPrefs.SetInt(Constant.KEY_SAVE_COIN, PlayerPrefs.GetInt(Constant.KEY_SAVE_COIN) + playerBall.coinInLevel);
        else
        {
            PlayerPrefs.SetInt(Constant.KEY_SAVE_COIN, playerBall.coinInLevel);
        }
        EventDispatcher.Instance.PostEvent(EventID.UpdateCoin);
    }

    public void NextLevel()
    {
        levelNumber += 1;
        SetLevelNumber(levelNumber);
        LoadLevel();
    }

    public int GetPlayerCoin()
    {
        return PlayerPrefs.GetInt(Constant.KEY_SAVE_COIN);
    }

    private void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }

    public void ShowFloatingText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

}
