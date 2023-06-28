using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemState : PlayerState
{
    public ItemState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, string animName) : base(control, machine, anim, rigid, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
