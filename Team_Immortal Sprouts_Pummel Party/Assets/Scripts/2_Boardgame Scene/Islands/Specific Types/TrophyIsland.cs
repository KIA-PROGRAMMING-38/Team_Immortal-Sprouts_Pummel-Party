using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyIsland : Island
{
    

    private void Start()
    {
        InitPositionSettings().Forget();
    }
    public override void ActivateIsland(Transform playerTransform = null)
    {
        // 황금알 주는 로직
    }
}
