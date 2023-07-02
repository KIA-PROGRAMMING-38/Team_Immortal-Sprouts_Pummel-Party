using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RealPistol : RealItem, IControllable
{
    public void OnJoystickInput(InputAction.CallbackContext context)
    {
        
    }

    public override void OnTimeOut()
    {
        // 시간 다되었을떄 로직
    }

    public override void Use(BoardPlayerController player = null)
    {
        // 사용 로직
    }
}
