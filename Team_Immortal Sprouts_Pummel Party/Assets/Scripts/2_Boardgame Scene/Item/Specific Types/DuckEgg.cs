using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DuckEgg : ControllItem
{
    /// <summary>
    /// 인벤토리에서 사용할 아이템으로 선택되었을 때의 동작
    /// </summary>
    public override void SetForUse(BoardgamePlayer usePlayer)
    {
        base.SetForUse(usePlayer);
    }

    /// <summary>
    /// 플레이어가 아이템 조작 후 사용 버튼을 입력했을 때 처리할 동작
    /// </summary>
    public override void Use()
    {
        base.Use();
    }

    /// <summary>
    /// Use 호출 전 턴 제한시간이 종료되었을 때의 동작
    /// </summary>
    public override void OnTimeOut()
    {

    }

    /// <summary>
    /// Joystick 입력으로 받아온 Vector2 MoveDir을 이용해 처리할 움직임
    /// </summary>
    public override void OnJoystickInput(InputAction.CallbackContext context)
    {
        if(!context.performed)
        {
            return;
        }

        base.OnJoystickInput(context);

        // TODO: 움직임 구현, 입력 방향은 MoveDir 사용
    }

    /// <summary>
    /// 아이템 사용 버튼 입력 시 처리할 동작
    /// </summary>
    public override void OnUseButtonInput(InputAction.CallbackContext context)
    {
        if(!context.canceled) 
        {
            return;
        }

        base.OnUseButtonInput(context);

        Use();

        // TODO: 사용 버튼 입력 시 동작 구현
    }
}
