using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;
using DG.Tweening;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Speed = 5;
    [SerializeField] private float RunningSpeed = 7;
    [SerializeField] private float WalkingSpeed = 2;
    [SerializeField] private float JumpForce = 5;
    private PlayerController Controller;
    [Header("Gravity")]
    [SerializeField] private float GroundYOffset;
    [SerializeField] private LayerMask GroundMask;
    Vector3 SpherePos;

    [SerializeField] float Gravity = -9.81f * 2;
    Vector3 Velocity;

    private void Start()
    {
        Speed = RunningSpeed;
        Controller = GetComponent<PlayerController>();
        UserInputController.Instance.OnJumpBtnClick += Jump;
        UserInputController.Instance.OnControlJoystick += MovePlayer;
        UserInputController.Instance.OnBtnShootDown += StartAiming;
        UserInputController.Instance.OnBtnShootUp += CancelAiming;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnJumpBtnClick -= Jump;
        UserInputController.Instance.OnControlJoystick -= MovePlayer;
        UserInputController.Instance.OnBtnShootDown -= StartAiming;
        UserInputController.Instance.OnBtnShootUp -= CancelAiming;
    }
    private void Update()
    {
        ApplyGravity();
    }
    private void MovePlayer(float hzInput, float vInput)
    {
        Vector2 MoveInput = new Vector2(hzInput, vInput);
        Vector2 Direction = (new Vector2(hzInput, vInput)).normalized;
        if (MoveInput.magnitude > 0.1f)
        {
            if (Controller.IsAiming == false)
            {
                float rotation = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg + Controller.MyCamera.transform.eulerAngles.y;
                transform.eulerAngles = Vector3.up * rotation;
                Controller.charController.Move(transform.forward * Speed * Time.deltaTime);
            }
            else
            {
                Controller.charController.Move((transform.forward * vInput + transform.right * hzInput) * Speed * Time.deltaTime);

            }

            Controller.ChangeMovementState(PlayerMovementState.running);


        }
        else
        {
            Controller.ChangeMovementState(PlayerMovementState.idle);
        }

    }
    private void Jump()
    {
        if (IsGrounded())
        {
            Velocity.y = JumpForce;
            Controller.StartJump();
            UserInputController.Instance.OnBtnShootUp();
        }

    }
    public bool IsGrounded()
    {
        SpherePos = transform.position - Vector3.up * GroundYOffset;
        if (Physics.CheckSphere(SpherePos, Controller.charController.radius - 0.05f, GroundMask))
        {
            return true;
        }
        return false;
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

        Controller.charController.Move(Velocity * Time.deltaTime);
    }

    public void StartAiming()
    {
        Speed = WalkingSpeed;
    }
    public void CancelAiming()
    {
        Speed = RunningSpeed;
    }
}
