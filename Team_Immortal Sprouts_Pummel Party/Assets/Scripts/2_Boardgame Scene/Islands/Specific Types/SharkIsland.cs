using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkIsland : Island
{
    void Start()
    {
        InitPositionSettings().Forget();
    }

    public override void ActivateIsland()
    {
        // 상어 뛰게 하는 로직
    }
}
