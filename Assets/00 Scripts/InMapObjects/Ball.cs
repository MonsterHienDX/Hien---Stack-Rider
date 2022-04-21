using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isCollected;
    public bool isLoseInMap;
    public bool rollAhead;
    [SerializeField] private GameObject meshGO;
    public Material material;
    private void Start()
    {
        isLoseInMap = false;
    }

    private void Update()
    {
        if (isLoseInMap || !isCollected) return;
        if (rollAhead)
        {
            meshGO.transform.Rotate(-PlayerBall.instance.GetSpeed() * PlayerBall.instance.ballRotateRateSpeed, 0, 0, Space.World);
        }
        else
        {
            meshGO.transform.Rotate(PlayerBall.instance.GetSpeed() * PlayerBall.instance.ballRotateRateSpeed, 0, 0, Space.World);
        }
    }

    public void SetColor(Material ballMaterial)
    {
        meshGO.GetComponent<MeshRenderer>().material = ballMaterial;
        this.material = ballMaterial;
    }

    public void ReverseRollDir(int ballAmount)
    {

        // this.rollAhead = ballAmount % 2 == 1;
        this.rollAhead = !this.rollAhead;
    }

}
