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

    // TODO: 추후 턴매니저 이벤트로 변경
    private void OnEnable()
    {
        ItemUseTest.OnTimeOut.RemoveListener(OnTimeOut);
        ItemUseTest.OnTimeOut.AddListener(OnTimeOut);
    }

    private void OnDisable()
    {
        ItemUseTest.OnTimeOut.RemoveListener(OnTimeOut);
    }

    // TODO: 턴매니저 턴 종료 이벤트 구독, 포톤 연결시 파괴되는 방식 변경
    public abstract void OnTimeOut();
}
