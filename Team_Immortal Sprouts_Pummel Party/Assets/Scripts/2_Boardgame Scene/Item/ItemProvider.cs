using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemProvider
{
    public static ItemData[] ItemTable = Resources.LoadAll<ItemData>("ItemData");
    
    private static int totalWeight;

    private static int getRandomNumber()
    {
        if(totalWeight == 0)
        {
            foreach (ItemData item in ItemTable)
            {
                totalWeight += item.Weight;
            }
        }

        int randomNumber = (int)(1f + Random.value * (totalWeight - 1) + 0.5f);

        Debug.Log($"선정된 랜덤 숫자: {randomNumber}");
        return randomNumber;
    }

    private static ItemData getRandomItem()
    {
        ItemData item = null;
        int weight = 0;

        int randomNumber = getRandomNumber();

        for(int i = 0; i < ItemTable.Length; ++i)
        {
            weight += ItemTable[i].Weight;
            if(randomNumber <= weight)
            {
                item = ItemTable[i];
                break;
            }
        }

        return item;
    }

    public static void Test()
    {
        int[] counts = new int[8];

        for(int i = 0; i < 1000; ++i)
        {
            ItemData item = getRandomItem();
            counts[item.Id] += 1;
        }

        int index = 0;

        foreach(int count in counts)
        {
            Debug.Log($"{index++}: {count}");
        }
    }
}
