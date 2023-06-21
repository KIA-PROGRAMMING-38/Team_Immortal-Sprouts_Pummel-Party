using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIsland : Island
{
    private ItemData givenItem;

    void Start()
    {
        InitPositionSettings().Forget();
    }

    /// <summary>
    /// 아이템섬을 활성화, 플레이어가 턴에서 최종 도착한 위치가 아이템섬일 경우 호출
    /// </summary>
    public async UniTask<bool> Activate(BoardgamePlayer player)
    {
        await UniTask.Delay(1000);

        ItemProvider.GiveRandomItemTo(player, out givenItem);

        await ShowItem(player);


        return true;
    }

    [SerializeField] private BoardgamePlayer me;  // TODO: 받은 사람, 구경하는 사람 다르게 보이도록하는 거 고려해서 추가하기
    private async UniTask<bool> ShowItem(BoardgamePlayer player)
    {
        return true;
    }
}
