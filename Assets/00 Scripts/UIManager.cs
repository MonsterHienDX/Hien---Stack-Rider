using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Text coinText;
    public Text LevelText;

    private int currentLevel;

    private void Awake()
    {
        currentLevel = PlayerPrefs.GetInt(Constant.KEY_LEVEL);
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.UpdateCoin, UpdateCoinUI);
        EventDispatcher.Instance.RegisterListener(EventID.LoadLevel, UpdateLevelUI);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.UpdateCoin, UpdateCoinUI);
        EventDispatcher.Instance.RemoveListener(EventID.LoadLevel, UpdateLevelUI);
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
            coinText.text = (GameManager.instance.GetPlayerCoin() + coinAmount).ToString();
        }
        else
            coinText.text = (GameManager.instance.GetPlayerCoin()).ToString();
    }

    private void UpdateLevelUI(object param = null)
    {
        currentLevel = (int)param;
        string nText = $"Level: {currentLevel}";
        LevelText.text = nText;
    }

}
