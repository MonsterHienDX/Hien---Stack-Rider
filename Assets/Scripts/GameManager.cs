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

    public int levelNumber;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isLose = false;
        Application.targetFrameRate = 60;
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

    }

    private int GetLevelNumber()
    {
        if (PlayerPrefs.HasKey(StringConstant.KEY_LEVEL))
        {
            return PlayerPrefs.GetInt(StringConstant.KEY_LEVEL, 0);
        }
        else
        {
            PlayerPrefs.SetInt(StringConstant.KEY_LEVEL, 1);
            return 1;
        }
    }

    // Set level number
    public void SetLevelNumber(int level)
    {
        PlayerPrefs.SetInt(StringConstant.KEY_LEVEL, level);
    }

    public void EndLevel(bool isWin)
    {
        EventDispatcher.Instance.PostEvent(EventID.EndLevel, isWin);
        playerBall.StopMove();
        currentLevel = null;
        if (isWin)
        {
            IncreaseCoin();
            NextLevel();
        }
        else
        {
            LoadLevel();
        }

    }

    private void IncreaseCoin()
    {
        if (PlayerPrefs.HasKey(StringConstant.KEY_SAVE_COIN))
            PlayerPrefs.SetInt(StringConstant.KEY_SAVE_COIN, PlayerPrefs.GetInt(StringConstant.KEY_SAVE_COIN) + playerBall.coinInLevel);
        else
        {
            PlayerPrefs.SetInt(StringConstant.KEY_SAVE_COIN, playerBall.coinInLevel);
        }
    }

    private void NextLevel()
    {
        levelNumber += 1;
        SetLevelNumber(levelNumber);
        LoadLevel();
    }

}
