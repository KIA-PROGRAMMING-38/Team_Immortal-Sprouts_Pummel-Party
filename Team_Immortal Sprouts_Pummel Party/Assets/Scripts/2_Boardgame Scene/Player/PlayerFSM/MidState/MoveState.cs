using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    public MoveState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
        playerController.OnConveyDiceResult.RemoveListener(changeToMoveStart);
        playerController.OnConveyDiceResult.AddListener(changeToMoveStart);
    }

    protected int moveCount = 0;
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void changeToMoveStart(int rouletteResult)
    {
        moveCount = rouletteResult;
        Debug.Log($"주사위 값 = {moveCount}");
        stateMachine.ChangeState(playerController.MoveStart);
    }
}
