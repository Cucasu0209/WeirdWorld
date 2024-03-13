using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputController : MonoBehaviour
{
    public static UserInputController Instance;

    public Action<float, float> OnControlJoystick;
    public Action<float, float> OnCameraAxisChange;
    public Action OnJumpBtnClick;
    public Action OnBtnShootDown;
    public Action OnBtnShootUp;
    public Action OnBtnSwingDown;
    public Action OnBtnSwingUp;

    private void Awake()
    {
        Instance = this;
    }
}
