using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DuckEgg : ControllItem
{
    /// <summary>
    /// �κ��丮���� ����� ���������� ���õǾ��� ���� ����
    /// </summary>
    public override void SetForUse(BoardgamePlayer usePlayer)
    {
        base.SetForUse(usePlayer);
    }

    /// <summary>
    /// �÷��̾ ������ ���� �� ��� ��ư�� �Է����� �� ó���� ����
    /// </summary>
    public override void Use()
    {
        base.Use();
    }

    /// <summary>
    /// Use ȣ�� �� �� ���ѽð��� ����Ǿ��� ���� ����
    /// </summary>
    public override void OnTimeOut()
    {

    }

    /// <summary>
    /// Joystick �Է����� �޾ƿ� Vector2 MoveDir�� �̿��� ó���� ������
    /// </summary>
    public override void OnJoystickInput(InputAction.CallbackContext context)
    {
        if(!context.performed)
        {
            return;
        }

        base.OnJoystickInput(context);

        // TODO: ������ ����, �Է� ������ MoveDir ���
    }

    /// <summary>
    /// ������ ��� ��ư �Է� �� ó���� ����
    /// </summary>
    public override void OnUseButtonInput(InputAction.CallbackContext context)
    {
        if(!context.canceled) 
        {
            return;
        }

        base.OnUseButtonInput(context);

        Use();

        // TODO: ��� ��ư �Է� �� ���� ����
    }
}
