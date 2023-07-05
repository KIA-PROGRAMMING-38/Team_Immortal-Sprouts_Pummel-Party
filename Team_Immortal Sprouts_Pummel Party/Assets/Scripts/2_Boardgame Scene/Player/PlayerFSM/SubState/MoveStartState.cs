using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveStartState : MoveState
{
    public MoveStartState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
        playerController.GetDesiredState<HoveringState>(BoardgamePlayerAnimID.HOVERING).OnChangeToMoveStart.RemoveListener(getRouletteResult);
        playerController.GetDesiredState<HoveringState>(BoardgamePlayerAnimID.HOVERING).OnChangeToMoveStart.AddListener(getRouletteResult);
    }

    private int rouletteResult = 0;
    public override void Enter()
    {
        base.Enter();
        ActivateIsland(); // 회전섬 또는 시작섬에 위치한다면 작동한다
        playerController.ChangeToDesiredState(BoardgamePlayerAnimID.MOVEINPROGRESS);
    }

    public override void Exit()
    {
        base.Exit();
        playerController.GetDesiredState<MoveInProgressState>(BoardgamePlayerAnimID.MOVEINPROGRESS).SetMoveCount(rouletteResult);
    }

    private void getRouletteResult(int rouletteOutput)
    {
        rouletteResult = rouletteOutput;
    }

    protected override void ActivateIsland() // 여기서 Activate할껀 회전섬
    {
        currentIsland.ActivateOnMoveStart(playerController.transform.GetChild(0).transform);
    }
}
