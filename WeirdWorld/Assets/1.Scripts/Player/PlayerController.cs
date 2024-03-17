using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator PlayerAnimator;
    public FollowPlayerCamera MyCamera;
    public CharacterController charController;
    public bool IsAiming = false;
    public bool IsClimbing = false;

    [Header("Gravity")]
    [SerializeField] public float Gravity = -50;
    public readonly float DefaultGravity = -50;
    [HideInInspector] public Vector3 Velocity;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private float GroundYOffset;
    [SerializeField] private LayerMask WallMask;

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
    private void Update()
    {
        ApplyGravity();
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
    public bool IsGrounded()
    {
        Vector3 SpherePos = transform.position - Vector3.up * GroundYOffset;
        if (Physics.CheckSphere(SpherePos, charController.radius - 0.05f, GroundMask))
        {
            return true;
        }
        return false;
    }
    public bool CheckWallInFront()
    {
        return (Physics.CheckSphere(transform.position + Vector3.up * charController.height / 2 + transform.forward * charController.radius * 1.2f, 0.4f, WallMask));
    }
    void ApplyGravity()
    {
        if (IsGrounded() == false)
        {
            Velocity.y += Gravity * Time.deltaTime;
        }
        else if (Velocity.y < 0)
        {
            Velocity.y = 0;
        }

        charController.Move(Velocity * Time.deltaTime);
    }
    public void SetAntiGravity()
    {
        Gravity = 0;
    }
    public void SetDefaultGravity()
    {
        PlayerAnimator.SetBool("IsClimbing", false);

        Gravity = DefaultGravity;
    }
    public void SetClimbAnim(float hzInput, float vInput)
    {
        PlayerAnimator.SetBool("IsClimbing", true);
        PlayerAnimator.SetFloat("hzInput", hzInput);
        PlayerAnimator.SetFloat("vInput", vInput);

    }
}

public enum PlayerMovementState { running, idle }