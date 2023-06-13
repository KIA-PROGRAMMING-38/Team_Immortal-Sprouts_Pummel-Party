using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : ControllItem
{
    private Transform _playerTransform;
    private Rigidbody _playerRigidbody;
    public override void SetForUse(BoardgamePlayer usePlayer)
    {
        base.SetForUse(usePlayer);
        
        _playerTransform = usePlayer.transform;
        _playerRigidbody = usePlayer.GetComponent<Rigidbody>();
        gameObject.transform.SetParent(_playerTransform, false);
    }

    public override void Use()
    {
        base.Use();
        // 회전은 알아서 움직이고 쏜 순간에 타겟을 맞춰야 함 회전 따라가는 거 따라서 그.. 판정하는 것도 바뀌지 말고
        // 회전하는 움직임 자체는 매 프레임 진행할 수 있도록..

        Recoil().Forget();
    }

    private const float ROTATION_TIME_AFTER_SHOOT = 0.01f;
    private const float ROTATION_TIME_FOR_RETURN = 0.05f;
    private async UniTaskVoid Recoil()
    {
        Quaternion originRotation = transform.rotation;
        Quaternion shootRotation = originRotation * Quaternion.Euler(30, 0, 0);

        float elapsedTime = 0f;

        while(elapsedTime <= ROTATION_TIME_AFTER_SHOOT)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(originRotation, shootRotation, elapsedTime / ROTATION_TIME_AFTER_SHOOT);

            await UniTask.Yield();
        }

        elapsedTime = 0f;
        while (elapsedTime <= ROTATION_TIME_FOR_RETURN)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(shootRotation, originRotation, elapsedTime / ROTATION_TIME_FOR_RETURN);

            await UniTask.Yield();
        }
    }

    public override void OnJoystickInput(InputAction.CallbackContext context)
    {
        if(!context.performed)
        {
            return;
        }

        base.OnJoystickInput(context);
        _playerRigidbody.MoveRotation(Quaternion.Euler(0f, Mathf.Atan2(MoveDir.x, MoveDir.y) * Mathf.Rad2Deg, 0f));
    }

    public override void OnUseButtonInput(InputAction.CallbackContext context)
    {

        if(!context.performed)
        {
            return;
        }

        base.OnUseButtonInput(context);

        Use();
    }

    public override void OnTimeOut()
    {

    }
}
