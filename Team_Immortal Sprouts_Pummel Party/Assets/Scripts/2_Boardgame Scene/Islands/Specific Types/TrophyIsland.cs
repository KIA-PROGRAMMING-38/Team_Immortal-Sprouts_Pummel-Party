using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyIsland : Island
{
    private void Start()
    {
        InitPositionSettings().Forget();
    }

    public override void ActivateOnMoveInProgress(Transform playerTransform = null)
    {
        Debug.Log("황금알 줄꺼임");
        // 황금알 주는 로직
    }
}
