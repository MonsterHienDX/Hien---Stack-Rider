using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public Ball playerBall;
    public TutorialManager tutorialManager;

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

    public void LoadLevel()
    {
        tutorialManager.EnableTutorial(true);
    }

}
