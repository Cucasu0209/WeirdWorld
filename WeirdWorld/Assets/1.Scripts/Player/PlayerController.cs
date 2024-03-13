using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator PlayerAnimator;
    public FollowPlayerCamera MyCamera;
    public Rigidbody Rigidbody;

    public void ChangeMovementState(PlayerMovementState state)
    {
        if (state == PlayerMovementState.running) PlayerAnimator.SetBool("isRunning", true);
        else PlayerAnimator.SetBool("isRunning", false);
    }
}

public enum PlayerMovementState { running, idle }