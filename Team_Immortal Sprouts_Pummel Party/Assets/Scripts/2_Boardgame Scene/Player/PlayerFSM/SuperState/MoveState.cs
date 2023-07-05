using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    public MoveState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
        bodyTransform = control.transform.GetChild(BODY_INDEX); // Body를 가져옴
    }
    private const int BODY_INDEX = 0;
    protected Transform bodyTransform;
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
        Quaternion start = bodyTransform.rotation;
        await ExtensionMethod.QuaternionLerpExtension(bodyTransform, start, forwardRotation, rotateTime);
    }


    protected virtual void ActivateIsland() { }
}
