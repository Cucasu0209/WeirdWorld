using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator PlayerAnimator;
    public FollowPlayerCamera MyCamera;
    public CharacterController charController;
    public bool IsAiming = false;

    private void Start()
    {
        UserInputController.Instance.OnBtnShootDown += StartAiming;
        UserInputController.Instance.OnBtnShootUp += CancelAiming;
    }

    private void OnDestroy()
    {
        UserInputController.Instance.OnBtnShootDown -= StartAiming;
        UserInputController.Instance.OnBtnShootUp -= CancelAiming;
    }
    public void ChangeMovementState(PlayerMovementState state)
    {
        if (state == PlayerMovementState.running) PlayerAnimator.SetBool("isRunning", true);
        else PlayerAnimator.SetBool("isRunning", false);
    }
    public void StartJump()
    {
        PlayerAnimator.SetTrigger("Jump");
    }
    public void StartAiming()
    {
        IsAiming = true;
        PlayerAnimator.SetBool("Aiming", IsAiming);
    }
    public void CancelAiming()
    {
        IsAiming = false;
        PlayerAnimator.SetBool("Aiming", IsAiming);
    }
}

public enum PlayerMovementState { running, idle }