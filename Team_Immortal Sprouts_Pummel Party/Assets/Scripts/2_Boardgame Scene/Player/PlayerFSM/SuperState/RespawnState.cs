using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnState : PlayerState
{
    public RespawnState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        changeToHoveringState().Forget();
    }

    public override void Exit()
    {
        base.Exit();
        playerController.SetIsPlayerRespawned(false); // 다시 기본상태로 리셋
    }

    private async UniTaskVoid changeToHoveringState()
    {
        await UniTask.WaitUntil(() => playerController.IsPlayerRespawned());

        playerController.ChangeToDesiredState(BoardgamePlayerAnimID.HOVERING);
    }
}
