using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanelManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    public GameObject backGround;
    public GameObject loseGO;
    public GameObject winGO;


    public Button SkipLevel;
    public Button TryAgain;

    public Button x2Coin;
    public Button NoThanks;

    private void OnEnable()
    {
        SkipLevel.onClick.AddListener(GameManager.instance.NextLevel);

        TryAgain.onClick.AddListener(GameManager.instance.LoadLevel);

        x2Coin.onClick.AddListener(GameManager.instance.SetPlayerCoin);
        x2Coin.onClick.AddListener(GameManager.instance.SetPlayerCoin);
        x2Coin.onClick.AddListener(GameManager.instance.NextLevel);

        NoThanks.onClick.AddListener(GameManager.instance.SetPlayerCoin);
        NoThanks.onClick.AddListener(GameManager.instance.NextLevel);
    }

    private void OnDisable()
    {
        SkipLevel.onClick.RemoveAllListeners();

        TryAgain.onClick.RemoveAllListeners();

        x2Coin.onClick.RemoveAllListeners();

        NoThanks.onClick.RemoveAllListeners();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowPopup(bool isWin)
    {
        // backGround.GetComponent<Image>().enabled = true;
        backGround.SetActive(true);

        StartCoroutine(EnableButton(isWin));

        GameObject activeGO = isWin ? winGO : loseGO;
        foreach (Animation a in activeGO.transform.GetComponentsInChildren<Animation>())
        {
            Debug.LogWarning("Play anim: " + a.name);
            a.Play();
        }
    }

    IEnumerator EnableButton(bool isWin)
    {
        yield return new WaitForSeconds(.3f);
        winGO.SetActive(isWin);
        loseGO.SetActive(!isWin);
    }


    public void HidePopup()
    {
        // backGround.GetComponent<Image>().enabled = false;
        backGround.SetActive(false);
        winGO.SetActive(false);
        loseGO.SetActive(false);
    }

}
