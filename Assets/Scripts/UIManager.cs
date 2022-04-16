using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Text coinText;
    public Text LevelText;

    private int currentCoin;
    private int currentLevel;

    private void Awake()
    {
        currentCoin = PlayerPrefs.GetInt(StringConstant.KEY_SAVE_COIN);
        currentLevel = PlayerPrefs.GetInt(StringConstant.KEY_LEVEL);
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.UpdateCoin, UpdateCoinUI);
        EventDispatcher.Instance.RegisterListener(EventID.EndLevel, UpdateLevelUI);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.UpdateCoin, UpdateCoinUI);
        EventDispatcher.Instance.RemoveListener(EventID.EndLevel, UpdateLevelUI);
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
            // currentCoin += coinAmount;
            coinText.text = (currentCoin + coinAmount).ToString();
        }
        else
            coinText.text = (currentCoin).ToString();
    }

    private void UpdateLevelUI(object param = null)
    {
        bool win = (bool)param;
        if (win)
        {
            string nText = $"Level: {currentLevel + 1}";
        }
        else
        {

        }
    }

}
