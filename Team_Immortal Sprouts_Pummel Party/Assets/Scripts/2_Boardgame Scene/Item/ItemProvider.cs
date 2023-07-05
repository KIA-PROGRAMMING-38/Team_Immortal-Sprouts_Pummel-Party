using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemProvider
{
    public static FakeItemData[] ItemTable = Resources.LoadAll<FakeItemData>("ItemData");
    
    private static int totalWeight;

    //private static int getRandomNumber()
    //{
    //    if(totalWeight == 0)
    //    {
    //        foreach (ItemData item in ItemTable)
    //        {
    //            totalWeight += item.Weight;
    //        }
    //    }

    //    int randomNumber = (int)(1f + Random.value * (totalWeight - 1) + 0.5f);

    //    return randomNumber;
    //}

    //private static ItemData getRandomItem()
    //{
    //    ItemData item = null;
    //    int weight = 0;

    //    int randomNumber = getRandomNumber();

    //    for(int i = 0; i < ItemTable.Length; ++i)
    //    {
    //        weight += ItemTable[i].Weight;
    //        if(randomNumber <= weight)
    //        {
    //            item = ItemTable[i];
    //            break;
    //        }
    //    }

    //    return item;
    //}

    /// <summary>
    /// 파라미터로 전달된 플레이어에게 랜덤한 아이템을 지급
    /// </summary>
    /// <param name="player">아이템을 지급받을 플레이어</param>
    //public static void GiveRandomItemTo(BoardgamePlayer player)
    //{
    //    ItemData item = getRandomItem();
    //    player.Inventory.Add(item);
    //}

    /// <summary>
    /// 파라미터로 전달된 플레이어에게 랜덤한 아이템을 지급, 지급된 아이템을 반환
    /// </summary>
    //public static void GiveRandomItemTo(BoardgamePlayer player, out ItemData givenItem)
    //{
    //    ItemData item = getRandomItem();
    //    player.Inventory.Add(item);

    //    givenItem = item;
    //}
}
