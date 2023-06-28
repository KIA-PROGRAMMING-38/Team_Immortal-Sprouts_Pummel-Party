using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringState : IdleState
{
    public HoveringState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
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
