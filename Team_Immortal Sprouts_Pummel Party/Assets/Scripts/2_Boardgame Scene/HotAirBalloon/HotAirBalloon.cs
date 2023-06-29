using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotAirBalloon : RealItem, IControllable
{
    [SerializeField] private Transform playerBoardPosition;
    [SerializeField] private Light spotLight;
    [SerializeField] private BoxCollider detectCollider;

    [Header("----------------------Hot Air Balloon--------------------------------")]
    [SerializeField] [Range(1f, 2f)] private float balloonMoveSpeed = 1f;
    [SerializeField] private float downDistance = 4f;
    [SerializeField][Range(1f, 3f)] private float flyTime = 3f;

    [Header("----------------------Player Control--------------------------------")]
    [SerializeField][Range(1f, 3f)] private float boardTime = 1f;

    [SerializeField] private Transform playerTransform = null; // 인스펙터창에서 볼려고 SerializeField
    private Animator playerAnimator;
    [SerializeField] CinemachineVirtualCamera cam; // 지금은 넣어줬지만, 나중에는 virtualCam 참조 받아오는 식으로 해야함
    private Vector3 cameraOffset = new Vector3(0f, 4.5f, -6f); // 자연스러운 카메라 body offSet

    private void OnEnable()
    {
    }

    private void Awake()
    {
        initMoveTokenSettings();
        spotLight.color = defualtColor;
    }

    private CancellationTokenSource cancelResource;
    private CancellationTokenSource playResource;
    private CancellationToken token;
    private void initMoveTokenSettings()
    {
        playResource = new CancellationTokenSource();
        cancelResource = new CancellationTokenSource();
        cancelResource.Cancel();
        token = playResource.Token;
    }

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
    
    public async void OnUseButtonInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            spotLight.enabled = false;
            detectCollider.enabled = false;
            SetTargetPlayer(playerTransform);
            await BalloonSink();
            if (playerTransform != null)
            {
                await playerOnBoard();
                HoldPlayer().Forget();
            }
            await UniTask.Delay(TimeSpan.FromSeconds(1f)); // 자연스러운 대기를 위해 
            balloonDisappear().Forget();
            
        }
    }

    private void SetTargetPlayer(Transform playerTrans)
    {
        playerTransform = playerTrans;
        if (playerTransform != null)
        {
            playerAnimator = playerTransform.gameObject.GetComponent<Animator>();
            isHoldPlayer = true;
        }
    }

    public async UniTask BalloonSink()
    {
        float elapsedTime = 0f;
        Vector3 initialPos = transform.position;
        Vector3 targetPos = initialPos;
        targetPos.y -= downDistance;

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

    private bool isHoldPlayer = false;
    [SerializeField] [Range(0.5f, 3f)] private float disappearTime = 2f;
    [SerializeField] [Range(20f, 50f)] private float balloonUpFactor = 20f;
    private async UniTask balloonDisappear()
    {
        Vector3 initialPostion = transform.position;
        Vector3 targetPosition = initialPostion + Vector3.up * balloonUpFactor;
        
        float elapsedTime = 0f;
        while (elapsedTime <= disappearTime)
        {
            transform.position = Vector3.Lerp(initialPostion, targetPosition, elapsedTime / disappearTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();  
        }

        isHoldPlayer = false;
    }




    private async UniTask HoldPlayer()
    {
        while (isHoldPlayer)
        {
            playerTransform.position = playerBoardPosition.position;
            await UniTask.Yield();
        }
    }

    public void OnTimeOut()
    {
        // 뭔가 해줘야함
    }

    

    private Color defualtColor = Color.red;
    private Color detectColor = Color.green;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spotLight.color = detectColor;
            playerTransform = other.GetComponent<Transform>();
        }
    }

    


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spotLight.color = defualtColor;
            playerTransform = null;
        }
    }


    public override void Use()
    {
        throw new NotImplementedException();
    }
}
