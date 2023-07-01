using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealIsland : Island
{
    private void Start()
    {
        InitPositionSettings().Forget();
    }

    public override void ActivateOnMoveEnd(Transform playerTransform = null)
    {
        // 힐 해주는 로직
        Debug.Log("힐 해줄꺼임");

        // 힐이 끝나면
        playerTransform.GetComponent<BoardPlayerController>().ControlMoveFinished(true); //
    }
}