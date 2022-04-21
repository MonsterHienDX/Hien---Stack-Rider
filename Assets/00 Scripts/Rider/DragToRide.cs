using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragToRide : MonoBehaviour
{
    private Slider rider;
    void Awake()
    {
        rider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        rider.onValueChanged.AddListener(GameManager.instance.BallMoveLeftRight);
    }
    private void OnDisable()
    {
        rider.onValueChanged.RemoveListener(GameManager.instance.BallMoveLeftRight);
    }
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch screenTouch = Input.GetTouch(0);

            if (screenTouch.phase == TouchPhase.Moved)
            {
                rider.value += screenTouch.deltaPosition.x / 100;
                GameManager.instance.BallMoveLeftRight(rider.value);
            }
            if (screenTouch.phase == TouchPhase.Ended)
            {
                // isActive = false;
            }
        }
    }



}
