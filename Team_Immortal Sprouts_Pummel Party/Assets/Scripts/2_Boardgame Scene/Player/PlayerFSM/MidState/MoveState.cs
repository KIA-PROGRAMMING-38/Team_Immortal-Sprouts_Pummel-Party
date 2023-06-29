using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    public MoveState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
        
    }

    
    protected Island currentIsland = null;
    
    public override void Enter()
    {
        base.Enter();
        updateCurrentIsland(); // currentIsland 업데이트
    }

    public override void Exit()
    {
        base.Exit();
    }
    protected void updateCurrentIsland()
    {
        RaycastHit hit;
        Physics.Raycast(playerController.transform.position, Vector3.down, out hit, int.MaxValue, LayerMask.GetMask("Island"));
        currentIsland = hit.collider.gameObject.GetComponentInParent<Island>();
    }

    private readonly Quaternion forwardRotation = Quaternion.Euler(0f, 180f, 0f);
    protected float rotateTime = 1f;
    protected async UniTask lookForward()
    {
        Quaternion start = playerController.transform.rotation;
        await ExtensionMethod.QuaternionLerpExtension(playerController.transform, start, forwardRotation, rotateTime);
    }


    protected virtual void ActivateIsland() { }
}
