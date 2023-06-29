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

    // 사용 안하는 함수
    public async UniTask OnActiveHealIsland(BoardgamePlayer player)
    {
        //await UniTask.Delay(1500);

        //player.OnRecover(15);
    }

    public override void ActivateIsland(Transform playerTransform = null)
    {
        // 힐 해주는 로직
        Debug.Log("힐 해줄꺼임");

        // 힐이 끝나면
        playerTransform.GetComponent<BoardPlayerController>().ControlMoveFinished(true);
    }
}