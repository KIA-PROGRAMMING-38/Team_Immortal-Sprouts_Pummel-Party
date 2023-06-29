using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;

public class RespawnState : PlayerState
{
    public RespawnState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
        forwardDirection.Set(0f, 180f, 0f);
    }

    private Vector3 forwardDirection;
    public override void Enter()
    {
        base.Enter();
        playerController.transform.rotation = Quaternion.Euler(forwardDirection); // 정면 바라보게함
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
