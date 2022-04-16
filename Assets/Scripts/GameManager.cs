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
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isLose = false;
        Application.targetFrameRate = 60;
        LoadLevel();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isGamePlaying)
        {
            isGamePlaying = false;
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
    }
    public void EndLevel(bool win)
    {
        Debug.LogWarning("Player win: " + win);
        EventDispatcher.Instance.PostEvent(EventID.EndLevel, win);
        playerBall.StopMove();

        if (win)
        {
            if (PlayerPrefs.HasKey(StringConstant.KEY_SAVE_COIN))
                PlayerPrefs.SetInt(StringConstant.KEY_SAVE_COIN, PlayerPrefs.GetInt(StringConstant.KEY_SAVE_COIN) + playerBall.coinInLevel);
            else
            {
                PlayerPrefs.SetInt(StringConstant.KEY_SAVE_COIN, playerBall.coinInLevel);
            }
        }
        else
        {

        }

    }


}
