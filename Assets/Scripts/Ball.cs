using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform endPointTransform;

    public float speed;

    void Start()
    {
        speed = 0;
    }

    public void StartPlay()
    {
        speed = 0.3f;
    }

    void Update()
    {
        if (transform.position.z > endPointTransform.position.z)
        {
            transform.Translate(endPointTransform.position * Time.deltaTime * speed);
        }
        else
        {
            speed = 0;
        }
    }
}
