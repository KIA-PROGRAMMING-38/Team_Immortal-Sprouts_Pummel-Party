using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : Item, IControllable
{

    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    public override void SetForUse(BoardgamePlayer usePlayer)
    {
        base.SetForUse(usePlayer);
        
        playerTransform = usePlayer.transform;
        playerRigidbody = usePlayer.GetComponent<Rigidbody>();
        gameObject.transform.SetParent(playerTransform, false);
    }

    [SerializeField] private Transform shootPoint;
    public override void Use()
    {
        base.Use();

        RaycastHit hit;
        Physics.Raycast(shootPoint.position, transform.forward * -1, out hit, int.MaxValue);

        if(hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log($"맞은 플레이어: {hit.collider.name}");
        }

        recoil().Forget();
    }

    private const float ROTATION_TIME_AFTER_SHOOT = 0.01f;
    private const float ROTATION_TIME_FOR_RETURN = 0.05f;
    private async UniTaskVoid recoil()
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

        // TODO: 포톤 연결 후 PrefabPool로 변경
        Destroy(gameObject);
    }

    private Vector2 moveDir;
    public void OnJoystickInput(InputAction.CallbackContext context)
    {
        if(!context.performed)
        {
            return;
        }

        moveDir = context.ReadValue<Vector2>().normalized;
        playerRigidbody.MoveRotation(Quaternion.Euler(0f, Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg, 0f));
    }

    public void OnUseButtonInput(InputAction.CallbackContext context)
    {
        if(!context.performed)
        {
            return;
        }

        Use();
    }

    public void OnTimeOut()
    {
        Destroy(gameObject);
    }
}
