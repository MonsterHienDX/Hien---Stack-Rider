using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform mapBallManager;
    public float road_transf_scale_z;

    public float distanceStartToEnd;
    public Transform roadTransform;

    private void Awake()
    {
        road_transf_scale_z = roadTransform.localScale.z;
        distanceStartToEnd = endPoint.position.z - startPoint.position.z;
    }

}

