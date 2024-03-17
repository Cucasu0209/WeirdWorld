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
    private void MovePlayer(float hzInput, float vInput)
    {
        if (Controller.IsClimbing == false)
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
    }
    private void Jump()
    {
        if (Controller.IsGrounded() && Controller.CheckWallInFront() == false)
        {
            Controller.Velocity.y = JumpForce;
            Controller.StartJump();
            UserInputController.Instance.OnBtnShootUp();
        }

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
