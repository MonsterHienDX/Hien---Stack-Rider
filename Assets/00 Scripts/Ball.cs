using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isCollected;
    public bool isLoseInMap;
    public int orderNumber;
    [SerializeField] private GameObject meshGO;

    private void Start()
    {
        isLoseInMap = false;
    }

    private void Update()
    {
        if (isLoseInMap || !isCollected) return;
        if (orderNumber % 2 == 0)
        {
            meshGO.transform.Rotate(PlayerBall.instance.GetSpeed() * PlayerBall.instance.ballRotateRateSpeed, 0, 0, Space.World);
        }
        else
        {
            meshGO.transform.Rotate(-PlayerBall.instance.GetSpeed() * PlayerBall.instance.ballRotateRateSpeed, 0, 0, Space.World);
        }
    }

    public void SetColor(Material ballMaterial)
    {
        meshGO.GetComponent<MeshRenderer>().material = ballMaterial;
    }
}
