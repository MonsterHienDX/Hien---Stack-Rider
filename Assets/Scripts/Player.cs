using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    private SphereCollider ballCollider;
    private void Start()
    {
        ballCollider = ballTransform.gameObject.GetComponent<SphereCollider>();
    }
    void Update()
    {
    }
}
