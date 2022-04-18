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
