using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ControllItem : Item
{
    public Vector2 MoveDir;
    public virtual void OnJoystickInput(InputAction.CallbackContext context)
    {
        MoveDir = context.ReadValue<Vector2>().normalized;
    }

    public virtual void OnUseButtonInput(InputAction.CallbackContext context)
    {

    }

    // TODO: ���� �ϸŴ��� �̺�Ʈ�� ����
    private void OnEnable()
    {
        ItemUseTest.OnTimeOut.RemoveListener(OnTimeOut);
        ItemUseTest.OnTimeOut.AddListener(OnTimeOut);
    }

    private void OnDisable()
    {
        ItemUseTest.OnTimeOut.RemoveListener(OnTimeOut);
    }

    // TODO: �ϸŴ��� �� ���� �̺�Ʈ ����, ���� ����� �ı��Ǵ� ��� ����
    public abstract void OnTimeOut();
}
