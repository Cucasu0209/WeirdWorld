using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerClimbing : MonoBehaviour
{
    private PlayerController Controller;
    [SerializeField] private float ClimbingSpeed = 2;
    private void Start()
    {
        Controller = GetComponent<PlayerController>();
        UserInputController.Instance.OnJumpBtnClick += ClimbWall;
        UserInputController.Instance.OnControlJoystick += MovePlayer;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnJumpBtnClick -= ClimbWall;
        UserInputController.Instance.OnControlJoystick += MovePlayer;
    }


    private void Update()
    {
        Debug.Log(Controller.CheckWallInFront());
    }

    private void ClimbWall()
    {
        if (Controller.IsClimbing)
        {
            Controller.IsClimbing = false;
            Controller.SetDefaultGravity();
        }
        else if (Controller.CheckWallInFront())
        {
            Controller.IsClimbing = true;
            Controller.SetAntiGravity();
        }
    }
    private void MovePlayer(float hzInput, float vInput)
    {
        if (Controller.IsClimbing)
        {
            Controller.charController.Move((transform.up * vInput + transform.right * hzInput) * ClimbingSpeed * Time.deltaTime);
            Controller.SetClimbAnim(hzInput, vInput);
        }
    }
}
