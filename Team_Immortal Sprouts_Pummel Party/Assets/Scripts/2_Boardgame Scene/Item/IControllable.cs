using UnityEngine.InputSystem;

public interface IControllable
{
    void OnJoystickInput(InputAction.CallbackContext context);
    void OnUseButtonInput(InputAction.CallbackContext context);
    void OnTimeOut();
}
