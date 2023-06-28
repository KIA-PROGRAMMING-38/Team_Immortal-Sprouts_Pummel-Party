using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected BoardPlayerController controller;
    protected StateMachine stateMachine;
    protected Animator animator;
    protected Rigidbody rigidbody;
    private string animParameter;

    public PlayerState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, string animName)
    {
        controller = control;
        stateMachine = machine;
        animator = anim;
        rigidbody = rigid;
        animParameter = animName;
    }

    public virtual void Enter()
    {
        animator.SetBool(animParameter, true);
    }

    public virtual void Exit()
    {
        animator.SetBool(animParameter, false);
    }
}
