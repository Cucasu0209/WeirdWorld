using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Speed = 5;
    private PlayerController Controller;

    private void Start()
    {
        Controller = GetComponent<PlayerController>();
        UserInputController.Instance.OnJumpBtnClick += OnJump;
        UserInputController.Instance.OnControlJoystick += MovePlayer;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnJumpBtnClick -= OnJump;
        UserInputController.Instance.OnControlJoystick -= MovePlayer;
    }
    private void MovePlayer(float hzInput, float vInput)
    {
        Vector2 MoveInput = new Vector2(hzInput, vInput);
        Vector2 Direction = (new Vector2(hzInput, vInput)).normalized;
        if (MoveInput.magnitude > 0.1f)
        {
            float rotation = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg + Controller.MyCamera.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * rotation;
            Controller.ChangeMovementState(PlayerMovementState.running);
            transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);

        }
        else
        {
            Controller.ChangeMovementState(PlayerMovementState.idle);
        }

    }
    private void OnJump()
    {

    }
}
