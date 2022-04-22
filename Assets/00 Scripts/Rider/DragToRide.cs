using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragToRide : MonoBehaviour
{
    private Slider rider;
    private bool isActive;
    void Awake()
    {
        rider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        rider.onValueChanged.AddListener(GameManager.instance.BallMoveLeftRight);
        EventDispatcher.Instance.RegisterListener(EventID.EndLevel, DisableRider);
        EventDispatcher.Instance.RegisterListener(EventID.LoadLevel, EnableRider);
        EventDispatcher.Instance.RegisterListener(EventID.LoadLevel, ResetPlayerToCenterRoad);
    }
    private void OnDisable()
    {
        rider.onValueChanged.RemoveListener(GameManager.instance.BallMoveLeftRight);
        EventDispatcher.Instance.RemoveListener(EventID.EndLevel, DisableRider);
        EventDispatcher.Instance.RemoveListener(EventID.LoadLevel, EnableRider);
        EventDispatcher.Instance.RemoveListener(EventID.LoadLevel, ResetPlayerToCenterRoad);
    }

    private void DisableRider(object param = null)
    {
        this.isActive = false;
        rider.interactable = false;
    }
    private void EnableRider(object param = null)
    {
        this.isActive = true;
        rider.interactable = true;
    }

    private void ResetPlayerToCenterRoad(object param = null)
    {
        rider.value = 0;
    }

    void Update()
    {
        if (Input.touchCount == 1 && isActive)
        {
            Touch screenTouch = Input.GetTouch(0);

            if (screenTouch.phase == TouchPhase.Moved)
            {
                rider.value += screenTouch.deltaPosition.x / 100;
                GameManager.instance.BallMoveLeftRight(rider.value);
            }
        }
    }



}
