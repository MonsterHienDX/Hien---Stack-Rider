using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    public float height = 12.5f;
    void LateUpdate()
    {
        this.transform.LookAt(player);
    }
}
