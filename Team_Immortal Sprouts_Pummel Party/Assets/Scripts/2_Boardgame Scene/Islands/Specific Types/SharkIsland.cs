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
        Debug.Log("공격 상어 발동함");
        shark.JumpAttack();
    }
}
