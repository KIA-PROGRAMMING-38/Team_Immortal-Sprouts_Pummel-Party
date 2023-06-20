using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public BoardgamePlayer TargetPlayer;

    private void OnEnable()
    {
        TargetPlayer.GetDamage(5);
        Debug.Log($"{TargetPlayer.Hp}");
    }
}
