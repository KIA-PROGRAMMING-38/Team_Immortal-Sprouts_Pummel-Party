using UnityEngine.InputSystem;

public interface IControllable
{
    void OnJoystickInput(InputAction.CallbackContext context);
    
}
