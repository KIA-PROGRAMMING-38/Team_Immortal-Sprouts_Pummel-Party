using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : PlayerState
{
    public DamagedState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        CameraTrace.DisconnectFollow(); // 데미지를 입으면 추적을 멈춤
        playerController.PlayDamagedParticle();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
