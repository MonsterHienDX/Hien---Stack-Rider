using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool isShowTutorial;

    public void EnableTutorial(bool enable)
    {
        gameObject.SetActive(enable);
    }
}
