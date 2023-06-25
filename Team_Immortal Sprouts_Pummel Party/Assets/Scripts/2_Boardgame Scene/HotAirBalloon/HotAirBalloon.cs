using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotAirBalloon : MonoBehaviour
{
    [SerializeField] private Transform playerBoardPosition;
    private Transform playerTransform;

    [Header("----------------------Hot Air Balloon--------------------------------")]
    [SerializeField] private float downDistance = 4f;
    [SerializeField] [Range(1f, 3f)] private float flyTime = 3f;

    [Header("----------------------Player Control--------------------------------")]
    [SerializeField] [Range(1f, 3f)] private float boardTime = 1f;

    private Animator playerAnimator;

    private void OnEnable()
    {
    }

    private void Start()
    {
        
    }

    

    private async void Update()
    {
    }

    public void SetPlayer(Transform playerTrans)
    {
        playerTransform = playerTrans;
        playerAnimator = playerTransform.gameObject.GetComponent<Animator>();
    }
    
    public void OnMoveVirtualJoystick(InputAction.CallbackContext context)
    {
        Vector2 inputs = context.ReadValue<Vector2>();

    }

    public void OnTouchActionButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ApproachPlayer().Forget();
        }
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




}
