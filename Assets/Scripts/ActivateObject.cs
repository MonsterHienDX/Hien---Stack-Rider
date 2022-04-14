using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.touchCount > 0
        // && Input.touches[0].phase == TouchPhase.Began
        )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Rider")
                {
                    var objectScript = hit.collider.GetComponent<DragToRide>();
                }
            }

        }
    }
}
