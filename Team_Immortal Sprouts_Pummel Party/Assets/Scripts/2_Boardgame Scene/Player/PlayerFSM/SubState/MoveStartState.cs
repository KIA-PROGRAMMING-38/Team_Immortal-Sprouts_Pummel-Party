using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveStartState : MoveState
{
    public MoveStartState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
        playerController.Hovering.OnChangeToMoveStart.RemoveListener(getRouletteResult);
        playerController.Hovering.OnChangeToMoveStart.AddListener(getRouletteResult);
    }

    private int rouletteResult = 0;
    public override void Enter()
    {
        base.Enter();
        ActivateIsland(); // 회전섬 또는 시작섬에 위치한다면 작동한다
        stateMachine.ChangeState(playerController.MoveInProgress);
    }

    public override void Exit()
    {
        base.Exit();
        playerController.MoveInProgress.SetMoveCount(rouletteResult);
    }

    private void getRouletteResult(int rouletteOutput)
    {
        rouletteResult = rouletteOutput;
    }

    protected override void ActivateIsland() // 여기서 Activate할껀 회전섬
    {
        if (currentIsland is RotationIsland)
        {
            IActiveIsland island = currentIsland.GetComponent<IActiveIsland>();
            island.ActivateIsland(playerController.transform);
            playerController.MoveInProgress.canMove = false;
            Debug.Log("회전섬임");
        }
        else if(currentIsland.CompareTag("StartIsland"))
        {
            Debug.Log("시작섬임");
            playerController.isEggGettable = false;
        }
    }
}
