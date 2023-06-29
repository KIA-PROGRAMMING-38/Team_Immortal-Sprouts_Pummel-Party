using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoveringState : PlayerState
{
    public HoveringState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
        playerController.OnRouletteStopped.RemoveListener(changeToMoveStart);
        playerController.OnRouletteStopped.AddListener(changeToMoveStart);
    }

    public UnityEvent<int> OnChangeToMoveStart = new UnityEvent<int>();

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    private void changeToMoveStart(int rouletteResult)
    {
        OnChangeToMoveStart?.Invoke(rouletteResult); // 주사위수를 먼저 전달해줘야함
        if (rouletteResult != 0)
        {
            playerController.ControlCanMove(true);
            stateMachine.ChangeState(playerController.MoveStart);
        }
        
    }
}
