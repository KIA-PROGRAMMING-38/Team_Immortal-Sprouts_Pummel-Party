using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoveringState : IdleState
{
    public HoveringState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
        waitAddListener();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void changeToMovingState()
    {
        stateMachine.ChangeState(playerController.MovingState);
    }

    private async UniTaskVoid waitAddListener()
    {
        await UniTask.WaitUntil(() => playerController.MovingState != null);

        playerController.MovingState.OnSetMoveCount.RemoveListener(changeToMovingState);
        playerController.MovingState.OnSetMoveCount.AddListener(changeToMovingState);
    }
}
