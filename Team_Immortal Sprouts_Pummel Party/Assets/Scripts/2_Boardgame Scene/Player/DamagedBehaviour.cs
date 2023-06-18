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
        // ��ƼŬ

        if(player.Hp <= 0)
        {
            animator.SetBool(BoardgamePlayerAnimID.DIE, true);
            player.Hp = 30;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(BoardgamePlayerAnimID.DAMAGED, false);

        // TODO: ���� Die ���� ���� �� Die ���¸� ��� �� ó��
        animator.SetBool(BoardgamePlayerAnimID.DIE, false);
    }
}
