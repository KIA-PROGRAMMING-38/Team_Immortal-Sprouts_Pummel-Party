using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealIsland : Island
{
    void Start()
    {
        InitPositionSettings().Forget();
    }

    /// <summary>
    /// �� ���� Ȱ��ȭ, �÷��̾ �Ͽ��� ���� ������ ��ġ�� �� ���� ��� ȣ��
    /// </summary>
    public async UniTask OnActiveHealIsland(BoardgamePlayer player)
    {
        await UniTask.Delay(1500);

        player.OnRecover(15);
    }
}