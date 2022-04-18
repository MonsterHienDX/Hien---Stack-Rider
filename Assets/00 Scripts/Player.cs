using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ChangeCharacterState, UpdateAnimState);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.ChangeCharacterState, UpdateAnimState);
    }

    private void UpdateAnimState(object param = null)
    {
        int state = (int)param;
        animator.SetInteger("characterState", state);
    }

}
