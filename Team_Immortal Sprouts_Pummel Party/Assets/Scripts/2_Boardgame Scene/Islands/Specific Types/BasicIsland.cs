using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicIsland : Island
{
    public override void ActivateIsland()
    {
        // 아무것도 안함
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitPositionSettings().Forget();
    }

}
