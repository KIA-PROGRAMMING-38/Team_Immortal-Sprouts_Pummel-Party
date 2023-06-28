using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkIsland : Island
{
    private BaseShark shark;
    void Start()
    {
        InitPositionSettings().Forget();
        shark = GetComponentInChildren<BaseShark>();
    }

    public override void ActivateIsland(Transform playerTransform = null)
    {
        shark.JumpAttack();
    }
}
