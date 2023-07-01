using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEndState : MoveState
{
    public MoveEndState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
    }


    public bool isMoveFinished { get; set; } = false;    

    public override async void Enter()
    {
        base.Enter();
        playerController.ControlCanMove(false);
        await lookForward(); // 이동이 끝나면 앞을 봄
        ActivateIsland();
        changeToHoveringState().Forget();
    }

    public override void Exit()
    {
        base.Exit();
        isMoveFinished = false;
    }

    protected override void ActivateIsland()
    {
        //isMoveFinished = true; // 이걸 섬에서 해줘야할듯?
        currentIsland.ActivateOnMoveEnd();
    }

    private async UniTaskVoid changeToHoveringState()
    {
        await UniTask.WaitUntil(() => isMoveFinished);
        playerController.ChangeToDesiredState(BoardgamePlayerAnimID.HOVERING);
    }

    
}
