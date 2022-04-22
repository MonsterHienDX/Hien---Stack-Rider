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
    [SerializeField] public EndGamePanelManager endGamePanelManager;
    [SerializeField] private FloatingTextManager floatingTextManager;
    [SerializeField] private BallManager ballManager;
    public int levelNumber;
    [SerializeField] private MeshRenderer quadMeshRenderer;
    [SerializeField] private List<Material> bgrMaterial;
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
            currentLevel = Instantiate<GameObject>(levelPrefabs[levelNumber - 1], levelRoot);
        else
        {
            currentLevel = Instantiate<GameObject>(levelPrefabs[UnityEngine.Random.Range(0, levelPrefabs.Count)], levelRoot);
        }

        currentLevel.transform.SetParent(levelRoot);

        LevelInfo levelInfo = currentLevel.GetComponent<LevelInfo>();
        playerBall.Init(levelInfo);
        ballManager.Init(levelInfo.ballMapContainer);

        EventDispatcher.Instance.PostEvent(EventID.LoadLevel, levelNumber);

        endGamePanelManager.HidePopup();

        ChangeBackGroundColor(bgrMaterial[UnityEngine.Random.Range(0, bgrMaterial.Count)]);
    }

    private void ChangeBackGroundColor(Material material)
    {
        quadMeshRenderer.material = material;
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
            AudioManager.instance.PlayAudio(AudioName.congratulation);
            Vibrator.Vibrate(Constant.STRONG_VIBRATE);
            EventDispatcher.Instance.PostEvent(EventID.ChangeCharacterState, Constant.WIN);
            StartCoroutine(WaitBallsExplodeThenPopup());
            SetPlayerScore(playerBall.coinInLevel);
        }
        else
        {
            Vibrator.Vibrate(Constant.WEAK_VIBRATE);
            EventDispatcher.Instance.PostEvent(EventID.ChangeCharacterState, Constant.LOSE);
            endGamePanelManager.ShowPopup(isWin);
        }
    }

    private IEnumerator WaitBallsExplodeThenPopup()
    {
        yield return new WaitForSeconds(Constant.DELAY_TO_DESTROY_BALL - 1.5f);
        playerBall.DestroyBallWhenWinAndShowPopup();
    }

    public void SetPlayerCoin()
    {
        if (PlayerPrefs.HasKey(Constant.KEY_SAVE_COIN))
            PlayerPrefs.SetInt(Constant.KEY_SAVE_COIN, PlayerPrefs.GetInt(Constant.KEY_SAVE_COIN) + playerBall.coinInLevel);
        else
        {
            PlayerPrefs.SetInt(Constant.KEY_SAVE_COIN, playerBall.coinInLevel);
        }
        EventDispatcher.Instance.PostEvent(EventID.UpdateCoin);
    }

    public void SetPlayerScore(int scoreAmount)
    {
        if (PlayerPrefs.HasKey(Constant.KEY_SAVE_SCORE))
            PlayerPrefs.SetInt(Constant.KEY_SAVE_SCORE, PlayerPrefs.GetInt(Constant.KEY_SAVE_SCORE) + scoreAmount);
        else
        {
            PlayerPrefs.SetInt(Constant.KEY_SAVE_SCORE, 1);
        }
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

    public int GetPlayerScore()
    {
        return PlayerPrefs.GetInt(Constant.KEY_SAVE_SCORE);
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
