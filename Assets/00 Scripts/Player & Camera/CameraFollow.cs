using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    public float distancePlayerToCam;

    void LateUpdate()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + Mathf.Abs(distancePlayerToCam));
    }
}
