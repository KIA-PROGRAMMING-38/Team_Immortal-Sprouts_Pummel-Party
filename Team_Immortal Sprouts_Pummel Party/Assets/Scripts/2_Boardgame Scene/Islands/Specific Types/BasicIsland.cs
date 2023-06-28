using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicIsland : Island
{
    private void Start()
    {
        InitPositionSettings().Forget();
    }
    public override void ActivateIsland(Transform playerTransform = null)
    {
        // 아무것도 안함
    }
}
