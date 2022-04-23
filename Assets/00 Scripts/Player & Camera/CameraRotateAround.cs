using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateAround : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    public bool isWin;
    [SerializeField] private Transform playerTransform;

    void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.EndLevel, CheckEndLevel);
        EventDispatcher.Instance.RegisterListener(EventID.LoadLevel, ResetCamera);
    }

    void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.EndLevel, CheckEndLevel);
        EventDispatcher.Instance.RemoveListener(EventID.LoadLevel, ResetCamera);
    }

    void Start()
    {
        isWin = false;
    }

    void Update()
    {
        if (isWin)
        {
            StartRotate();
        }
    }

    private void CheckEndLevel(object param = null)
    {
        isWin = (bool)param;

        if (isWin)
            this.transform.position = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
    }

    private void ResetCamera(object param = null)
    {
        isWin = false;
        this.transform.position = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        this.transform.rotation = new Quaternion();
    }

    private void StartRotate()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

}
