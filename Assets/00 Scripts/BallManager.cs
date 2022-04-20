using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{

    public Transform ballMapContainer;
    [SerializeField] List<Material> ballMaterialList;


    public Ball[] ballInMaps;

    public void Init(Transform ballMapContainer)
    {
        this.ballMapContainer = ballMapContainer;
        ballInMaps = ballMapContainer.GetComponentsInChildren<Ball>();
        SetColorForEachBall();
    }

    private void SetColorForEachBall()
    {
        for (int i = 0; i < ballInMaps.Length; i++)
        {
            ballInMaps[i].SetColor(ballMaterialList[UnityEngine.Random.Range(0, ballMaterialList.Count)]);
        }
    }


}
