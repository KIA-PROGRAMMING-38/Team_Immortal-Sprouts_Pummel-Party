using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemUseTest : MonoBehaviour
{
    public ItemData item;
    //public BoardgamePlayer player;
    //public BoardgamePlayer otherPlayer;

    void Start()
    {
        // 아이템 추가 테스트
        Invoke(nameof(GetTest), 1f);
        Invoke(nameof(OtherPlayerGet), 2f);
        Invoke(nameof(GetTest), 5f);

    }

    private void GetTest()
    {
        //player.Inventory.Add(item);
    }

    private void OtherPlayerGet()
    {
        //otherPlayer.Inventory.Add(item);
    }
}
