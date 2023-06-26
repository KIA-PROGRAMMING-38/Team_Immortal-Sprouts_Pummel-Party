using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotAirBalloon : MonoBehaviour, IControllable
{
    [SerializeField] private Transform playerBoardPosition;
    private Transform playerTransform;

    [Header("----------------------Hot Air Balloon--------------------------------")]
    [SerializeField] [Range(1f, 2f)] private float balloonMoveSpeed = 1f;
    [SerializeField] private float downDistance = 4f;
    [SerializeField][Range(1f, 3f)] private float flyTime = 3f;

    [Header("----------------------Player Control--------------------------------")]
    [SerializeField][Range(1f, 3f)] private float boardTime = 1f;

    private Animator playerAnimator;
    [SerializeField] CinemachineVirtualCamera cam;

    private void OnEnable()
    {
        CameraTrace.Connect(transform, cam);
    }

    private void Start()
    {
        playResource = new CancellationTokenSource();
        cancelResource = new CancellationTokenSource();
        cancelResource.Cancel();
        token = playResource.Token;
    }



    public void SetPlayer(Transform playerTrans)
    {
        playerTransform = playerTrans;
        playerAnimator = playerTransform.gameObject.GetComponent<Animator>();
    }

    public async UniTask ApproachPlayer()
    {
        float elapsedTime = 0f;
        Vector3 initialPos = transform.position;
        Vector3 targetPos = initialPos;
        targetPos.y = targetPos.y - downDistance;

        while (elapsedTime <= flyTime)
        {
            transform.position = Vector3.Lerp(initialPos, targetPos, elapsedTime / flyTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }


    private async UniTask playerOnBoard()
    {
        //playerAnimator.SetBool(BoardgamePlayerAnimID.IS_MOVING, true);
        float elapsedTime = 0f;
        Vector3 initialPos = playerTransform.position;
        Vector3 targetPos = playerBoardPosition.position;
        while (elapsedTime <= boardTime)
        {
            playerTransform.position = Vector3.Lerp(initialPos, targetPos, elapsedTime / boardTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private CancellationTokenSource cancelResource;
    private CancellationTokenSource playResource;
    private CancellationToken token;

    public void OnJoystickInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            token = playResource.Token;
            balloonMovement().Forget();
        }
        else if (context.performed)
        {
            Vector2 inputs = context.ReadValue<Vector2>();
            frontInput = inputs.y;
            sideInput = inputs.x;
        }
        else if (context.canceled)
        {
            frontInput = sideInput = 0f;
            token = cancelResource.Token;
        }
    }


    private float frontInput;
    private float sideInput;
    private async UniTaskVoid balloonMovement()
    {
        while (true)
        {
            Vector3 newDirection = (transform.forward * frontInput + transform.right * sideInput);
            newDirection *= balloonMoveSpeed * Time.deltaTime;
            transform.Translate(newDirection);
            await UniTask.Yield(token); 
        }
    }


    public void OnUseButtonInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ApproachPlayer().Forget();
        }
    }

    public void OnTimeOut()
    {
        // 뭔가 해줘야함
    }
}
