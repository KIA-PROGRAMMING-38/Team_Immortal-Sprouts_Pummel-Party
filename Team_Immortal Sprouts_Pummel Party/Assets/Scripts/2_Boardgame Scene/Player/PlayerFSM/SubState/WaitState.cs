using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : IdleState
{
    public WaitState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        activateIsland();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void activateIsland()
    {
        RaycastHit hit;
        Physics.Raycast(playerController.transform.position, Vector3.down, out hit, int.MaxValue, LayerMask.GetMask("Island"));

        if (hit.collider != null)
        {
            IActiveIsland island = hit.collider.transform.parent.GetComponent<IActiveIsland>();
            island.ActivateIsland(playerController.transform);
        }
    }
}
