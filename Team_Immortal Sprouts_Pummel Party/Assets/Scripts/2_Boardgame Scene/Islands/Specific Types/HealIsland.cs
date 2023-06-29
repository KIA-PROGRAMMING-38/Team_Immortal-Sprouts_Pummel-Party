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

    /// <summary>
    /// 힐 섬을 활성화, 플레이어가 턴에서 최종 도착한 위치가 힐 섬일 경우 호출
    /// </summary>
    public async UniTask OnActiveHealIsland(BoardgamePlayer player)
    {
        await UniTask.Delay(1500);

        player.OnRecover(15);
    }

    public override void ActivateIsland(Transform playerTransform = null)
    {
        // 힐 해주는 로직
        Debug.Log("힐 해줄꺼임");
    }
}