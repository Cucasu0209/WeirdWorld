using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class KeyBoardInput : MonoBehaviour
{
    float HzInput = 0;
    float vInput = 0;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) UserInputController.Instance.OnJumpBtnClick?.Invoke();

        if (Input.GetKeyDown(KeyCode.E)) UserInputController.Instance.OnBtnSwingDown?.Invoke();
        else if (Input.GetKeyUp(KeyCode.E)) UserInputController.Instance.OnBtnSwingUp?.Invoke();

        if (Input.GetMouseButtonDown(0)) UserInputController.Instance.OnBtnShootDown?.Invoke();
        else if (Input.GetMouseButtonUp(0)) UserInputController.Instance.OnBtnShootUp?.Invoke();


        if (Input.GetKey(KeyCode.W)) vInput = 1;
        else if (Input.GetKey(KeyCode.S)) vInput = -1;
        else vInput = 0;

        if (Input.GetKey(KeyCode.D)) HzInput = 1;
        else if (Input.GetKey(KeyCode.A)) HzInput = -1;
        else HzInput = 0;

        UserInputController.Instance.OnControlJoystick?.Invoke(HzInput, vInput);
    }
}
