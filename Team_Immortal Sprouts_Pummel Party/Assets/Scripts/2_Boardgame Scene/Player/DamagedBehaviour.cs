using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedBehaviour : StateMachineBehaviour
{
    private BoardgamePlayer player;

    private float elapsedTime;
    private const float ROTATE_TIME = 1.5f;
    private const float MAX_ROTATE_DEGREE = 360f;
    private const int ROTATE_COUNT = 4;
    private Vector3 startRotation;
    private Vector3 endRotation;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<BoardgamePlayer>();

        if(player.Hp <= 0)
        {
            animator.SetBool(BoardgamePlayerAnimID.DIE, true);
            // TODO: 추후 Die 상태 구현 시 Die 상태를 벗어날 때 처리
            player.Hp = 30;
        }

        elapsedTime = 0f;
        startRotation = player.transform.eulerAngles;
        endRotation = startRotation + new Vector3(0, MAX_ROTATE_DEGREE * ROTATE_COUNT, 0);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(elapsedTime < ROTATE_TIME)
        {
            elapsedTime += Time.deltaTime;
            float timeRatio = elapsedTime / ROTATE_TIME;
            Vector3 lerpRot = Vector3.Lerp(startRotation, endRotation, timeRatio);
            lerpRot.y %= MAX_ROTATE_DEGREE;
            player.transform.eulerAngles = lerpRot;
        }
        else
        {
            animator.SetBool(BoardgamePlayerAnimID.DAMAGED, false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // TODO: 추후 Die 상태 구현 시 Die 상태를 벗어날 때 처리
        animator.SetBool(BoardgamePlayerAnimID.DIE, false);
    }
}
