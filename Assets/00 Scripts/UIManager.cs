using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Text coinText;
    public Text levelText;
    public Text scoreText;
    private int currentLevel;
    private void Awake()
    {
        currentLevel = PlayerPrefs.GetInt(Constant.KEY_LEVEL);
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.UpdateCoin, UpdateCoinUI);
        EventDispatcher.Instance.RegisterListener(EventID.LoadLevel, UpdateLevelUI);
        EventDispatcher.Instance.RegisterListener(EventID.UpdateScore, UpdateScoreUI);
        EventDispatcher.Instance.RegisterListener(EventID.LoadLevel, ShowScoreUIWenEndLevel);
        EventDispatcher.Instance.RegisterListener(EventID.StartPlay, HideScoreUIWhenStartPlay);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.UpdateCoin, UpdateCoinUI);
        EventDispatcher.Instance.RemoveListener(EventID.LoadLevel, UpdateLevelUI);
        EventDispatcher.Instance.RemoveListener(EventID.UpdateScore, UpdateScoreUI);
        EventDispatcher.Instance.RemoveListener(EventID.LoadLevel, ShowScoreUIWenEndLevel);
        EventDispatcher.Instance.RemoveListener(EventID.StartPlay, HideScoreUIWhenStartPlay);
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    private void UpdateCoinUI(object param = null)
    {
        if (param != null)
        {
            int coinAmount = (int)param;
            coinText.text = (GameManager.instance.GetPlayerCoin() + coinAmount).ToString();
        }
        else
            coinText.text = (GameManager.instance.GetPlayerCoin()).ToString();
    }

    private void UpdateLevelUI(object param = null)
    {
        currentLevel = (int)param;
        string nText = $"Level: {currentLevel}";
        levelText.text = nText;
    }

    private void UpdateScoreUI(object param = null)
    {
        Text scoreAmountUI = scoreText.transform.GetChild(0).gameObject.GetComponent<Text>();
        int scoreAmount = (int)param;

        GameManager.instance.SetPlayerScore(scoreAmount);
        scoreAmountUI.text = (GameManager.instance.GetPlayerScore()).ToString();
    }

    private void HideScoreUIWhenStartPlay(object param = null)
    {
        scoreText.gameObject.SetActive(false);
    }

    private void ShowScoreUIWenEndLevel(object param = null)
    {
        scoreText.gameObject.SetActive(true);
    }

}
