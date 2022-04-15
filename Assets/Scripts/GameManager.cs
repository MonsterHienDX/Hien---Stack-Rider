using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerBall playerBall;
    public TutorialManager tutorialManager;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        LoadLevel();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            tutorialManager.EnableTutorial(false);
            playerBall.StartPlay();
        }
    }

    public void BallMoveLeftRight(float xPos)
    {
        playerBall.DragLeftRight(xPos);
    }

    public void LoadLevel()
    {
        tutorialManager.EnableTutorial(true);
    }

}
