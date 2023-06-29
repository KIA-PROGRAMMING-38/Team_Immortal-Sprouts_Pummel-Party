using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEndState : MoveState
{
    public MoveEndState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
    }

    public override async void Enter()
    {
        base.Enter();
        await lookForward(); // 이동이 끝나면 앞을 봄
        ActivateIsland();
    }

    public override void Exit()
    {
        
    }

    

    protected override void ActivateIsland() // 여기서 Activate 할껀 1) 상어섬 2) 힐섬
    {
        if (currentIsland is SharkIsland || currentIsland is HealIsland)
        {
            IActiveIsland island = currentIsland.GetComponent<IActiveIsland>();
            island.ActivateIsland(playerController.transform);
            Debug.Log($"{island} 임");
        }
    }
}
