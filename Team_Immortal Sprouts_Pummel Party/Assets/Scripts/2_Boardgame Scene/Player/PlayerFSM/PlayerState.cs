using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected BoardPlayerController playerController;
    protected StateMachine stateMachine;
    protected Animator playerAnimator;
    protected Rigidbody playerRigid;
    private int animParameter;

    public PlayerState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName)
    {
        playerController = control;
        stateMachine = machine;
        playerAnimator = anim;
        playerRigid = rigid;
        animParameter = animName;
    }

    public virtual void Enter()
    {
        playerAnimator.SetBool(animParameter, true);
    }

    public virtual void Exit()
    {
        playerAnimator.SetBool(animParameter, false);
    }


    protected virtual void activateIsland()
    {
        
    }
}
