using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isCollected;
    public bool isLoseInMap;
    private SphereCollider sphereCollider;
    private Animator animator;

    public int orderNumber;

    [SerializeField] private GameObject meshGO;
    private void Start()
    {
        isLoseInMap = false;
    }

    private void Update()
    {

        if (isLoseInMap || !isCollected) return;
        if (orderNumber % 2 == 1)
        {
            meshGO.transform.Rotate(PlayerBall.instance.GetSpeed() * PlayerBall.instance.ballRotateRateSpeed, 0, 0, Space.World);
        }
        else
        {
            meshGO.transform.Rotate(-PlayerBall.instance.GetSpeed() * PlayerBall.instance.ballRotateRateSpeed, 0, 0, Space.World);
        }
    }
}
