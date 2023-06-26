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
        CameraTrace.ConnectFollow(transform, cam);
        CameraTrace.ConnectLookAt(transform, cam);
        CameraTrace.ControlFollowOffset(cam, cameraOffset);
    }

    private void Start()
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

    

    

    

    private async UniTask playerOnBoard()
    {
        playerAnimator.SetBool(BoardgamePlayerAnimID.IS_MOVING, true);
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
            spotLight.enabled = false;
            detectCollider.enabled = false;
            SetTargetPlayer(playerTransform);
            BalloonSink().Forget();
        }
    }
    public async UniTask BalloonSink()
    {
        await UniTask.WaitUntil(() => playerTransform != null);

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
    public void OnTimeOut()
    {
        // 뭔가 해줘야함
    }

    private void SetTargetPlayer(Transform playerTrans)
    {
        playerTransform = playerTrans;
        playerAnimator = playerTransform.gameObject.GetComponent<Animator>();
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
}
