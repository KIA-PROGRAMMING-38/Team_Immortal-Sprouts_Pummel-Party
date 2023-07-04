using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartIsland : Island
{
    private void Start()
    {
        InitPositionSettings().Forget();
    }

    public override void ActivateOnMoveStart(Transform playerTransform = null)
    {
        BoardPlayerController player = playerTransform.parent.GetComponent<BoardPlayerController>();    
        player.SetPlayerEggable(false); // 황금알을 받을수 없게끔 처리
    }
}
