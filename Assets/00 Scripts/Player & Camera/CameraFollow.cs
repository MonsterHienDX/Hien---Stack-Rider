using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    public float distancePlayerToCam;
    [SerializeField] private CameraRotateAround camRotate;

    void LateUpdate()
    {
        if (camRotate.isWin)
        {
            this.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, player.transform.localPosition.z + Mathf.Abs(distancePlayerToCam));
        }
        else
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + Mathf.Abs(distancePlayerToCam));
        }
    }
}
