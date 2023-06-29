using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStartState : MoveState
{
    public MoveStartState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
    }

    
}
