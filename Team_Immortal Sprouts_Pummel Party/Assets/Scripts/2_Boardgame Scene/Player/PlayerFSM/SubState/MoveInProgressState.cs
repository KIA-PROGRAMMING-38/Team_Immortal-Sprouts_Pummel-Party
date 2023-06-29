using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;

public class MoveInProgressState : MoveState
{
    public MoveInProgressState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
        
    }

    private int moveCount = 0;
    
    public override async void Enter()
    {
        base.Enter();
        await move(); // 이동함
        
        updateCurrentIsland(); // 현재 섬을 업데이트함
        stateMachine.ChangeState(playerController.MoveEnd);
    }

    public override void Exit()
    {
        currentIsland.SetPlayerPresence(true); // 현재 섬에 존재함을 체크함
        playerController.isEggGettable = true; // 황금알 수령 가능
    }


    [SerializeField] private float moveTime = 1f;
    [SerializeField] private float jumpHeight = 5f;
    private Vector3 initialPos;
    private Vector3 controlPos;
    private Vector3 targetPos;
    private async UniTask move()
    {
        currentIsland.SetPlayerPresence(false); // 현재 섬에서 벗어나기때문에 없음 처리해줌
        while (moveCount != 0)
        {
            updateCurrentIsland(); // 현재 섬을 파악한다
            ActivateIsland(); // 트로피섬이라면 효과를 작동시킨다
            updateNextIsland(); // 다음섬을 정한다

            await UniTask.WaitUntil(() => canMove); // 움직임이 가능할때까지 기다린다
            moveCount -= 1;

            setMovePos(playerController.transform.position, nextPosition);
            
            Vector3 lookDirection = (targetPos - initialPos).normalized;
            await lookNextDestIsland(lookDirection); // 다음 섬을 쳐다본다

            await ExtensionMethod.SecondaryBezierCurve(playerController.transform, initialPos, controlPos, targetPos, moveTime);

            await UniTask.Yield();
        }
    }

    private Vector3 nextPosition;
    private void updateNextIsland()
    {
        if (moveCount >= 1)
        {
            nextPosition = currentIsland.GetNextPosition();
        }
        else if (moveCount == -1)
        {
            nextPosition = currentIsland.GetPrevPosition();
            moveCount = 1;
        }
        else // 0 이 나왔을 떄
        {
            nextPosition = currentIsland.GetCurrentPosition();
        }
    }

    private void setMovePos(Vector3 start, Vector3 end)
    {
        initialPos = start;
        targetPos = end;
        controlPos = Vector3.Lerp(initialPos, targetPos, 0.5f);
        controlPos.y += jumpHeight;
    }

    private async UniTask lookNextDestIsland(Vector3 dir)
    {
        Quaternion start = playerController.transform.rotation;
        Quaternion end = Quaternion.LookRotation(dir);

        await ExtensionMethod.QuaternionLerpExtension(playerController.transform, start, end, rotateTime);
    }

    public void SetMoveCount(int rouletteOutput) => moveCount = rouletteOutput;

    

    protected override async void ActivateIsland() // 여기서 Activate할껀 황금알섬
    {
        if (playerController.isEggGettable && currentIsland is TrophyIsland)
        {
            await lookForward();
            IActiveIsland island = currentIsland.GetComponent<IActiveIsland>();
            island.ActivateIsland(playerController.transform);
            canMove = false;
            Debug.Log("트로피섬임");
        }
    }
}
