using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedBehaviour : StateMachineBehaviour
{
    private BoardgamePlayer player;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<BoardgamePlayer>();

        if(player.Hp <= 0)
        {
            animator.SetBool(BoardgamePlayerAnimID.DIE, true);
            // TODO: 추후 Die 상태 구현 시 Die 상태를 벗어날 때 처리
            player.Hp = 30;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // TODO: 추후 Die 상태 구현 시 Die 상태를 벗어날 때 처리
        animator.SetBool(BoardgamePlayerAnimID.DIE, false);
    }
}
