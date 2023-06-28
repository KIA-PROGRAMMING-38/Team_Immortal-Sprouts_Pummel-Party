using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;

public class MovingState : DynamicState
{
    public MovingState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
        playerController.OnConveyDiceResult.RemoveListener(setMoveCount);
        playerController.OnConveyDiceResult.AddListener(setMoveCount);
    }

    private int moveCount = 0;
    private Island currentIsland = null;
    public override void Enter()
    {
        base.Enter();
        updateCurrentIsland();
        playerMovement().Forget();
    }

    public override void Exit()
    {
        base.Exit();
        moveCount = 0; // 이동횟수 리셋
    }

    public UnityEvent OnSetMoveCount = new UnityEvent();

    private void setMoveCount(int rouletteOutput)
    {
        moveCount = rouletteOutput;
        OnSetMoveCount?.Invoke();
    }

    private async UniTaskVoid playerMovement()
    {
        Debug.Log($"이동횟수 = {moveCount}");
        await checkDepartureIsland();
        checkReachableIsland();
        move().Forget();
    }

    private void updateCurrentIsland()
    {
        RaycastHit hit;
        Physics.Raycast(playerController.transform.position, Vector3.down * int.MaxValue, out hit, int.MaxValue, LayerMask.GetMask("Island"));

        if (hit.collider != null)
        {
            currentIsland = hit.collider.gameObject.GetComponentInParent<Island>();
            currentIsland.SetPlayerPresence(false);
            Debug.Log($"현재 섬 = {currentIsland.gameObject.name}");
        }
        else
        {
            Debug.Log("섬 감지 안됨");
        }
    }

    private bool canMoveOnDirectionIsland;
    private async UniTask checkDepartureIsland()
    {
        if (currentIsland is RotationIsland && moveCount >= 1)
        {
            currentIsland.GetComponent<RotationIsland>().PopUpDirectionArrow(playerController.transform);

            await UniTask.WaitUntil(() => currentIsland.GetComponent<RotationIsland>().GetRotationStatus() == true);
            canMoveOnDirectionIsland = true;
        }
    }

    private Vector3 destIslandPosition;
    private void checkReachableIsland()
    {
        if (moveCount >= 1)
        {
            destIslandPosition = currentIsland.GetNextPosition();
        }
        else if (moveCount == -1)
        {
            destIslandPosition = currentIsland.GetPrevPosition();
            moveCount = 1;
        }
        else // 0 이 나왔을 떄
        {
            destIslandPosition = currentIsland.GetCurrentPosition();
        }
    }

    [SerializeField] private float moveTime = 1f;
    [SerializeField] private float jumpHeight = 5f;
    private async UniTaskVoid move()
    {
        Vector3 initialPos;
        Vector3 targetPos;
        Vector3 midPos;

        if (currentIsland.GetCurrentPosition() == destIslandPosition)
        {
            await lookForward();
            return;
        }

        // 회전섬 다시 리셋하는 거 isPlayerPresent false일때 실행되도록 UniTask.WaitUntil 로 변경해야할듯?

        while (moveCount >= 1)
        {
            initialPos = playerController.transform.position;
            targetPos = destIslandPosition;
            midPos = Vector3.Lerp(initialPos, targetPos, 0.5f);
            midPos.y += jumpHeight;

            moveCount -= 1;
            Vector3 lookDirection = (targetPos- initialPos).normalized;
            await lookNextDestIsland(lookDirection);

            await ExtensionMethod.SecondaryBezierCurve(playerController.transform, initialPos, midPos, targetPos, moveTime);

            updateCurrentIsland();
            checkReachableIsland();
            await UniTask.Yield();
        }

        await lookForward();
        //이때가 이동이 끝난 시점임
        updateCurrentIsland();
        currentIsland.SetPlayerPresence(true); // 섬위에 존재함으로 설정
        stateMachine.ChangeState(playerController.WaitState);
    }



    [SerializeField] private float rotateTime = 1f;
    private async UniTask lookNextDestIsland(Vector3 dir)
    {
        // 회전타일에서부터 출발하는 턴에서는 회전하지 않음
        if (currentIsland is RotationIsland && canMoveOnDirectionIsland)
        {
            canMoveOnDirectionIsland = false;
        }

        Quaternion start = playerController.transform.rotation;
        Quaternion end = Quaternion.LookRotation(dir);

        await ExtensionMethod.QuaternionLerpExtension(playerController.transform, start, end, rotateTime);
    }


    
    private async UniTask lookForward()
    {
        Quaternion start = playerController.transform.rotation;
        Quaternion end = Quaternion.Euler(0f, 180f, 0f);

        await ExtensionMethod.QuaternionLerpExtension(playerController.transform, start, end, rotateTime);
    }
}
