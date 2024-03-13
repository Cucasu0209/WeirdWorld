using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileFeatureInput : MonoBehaviour
{
    public FixedJoystick FixedJoystick;
    private void Update()
    {
        UserInputController.Instance.OnControlJoystick?.Invoke(FixedJoystick.Horizontal, FixedJoystick.Vertical);
    }
}
