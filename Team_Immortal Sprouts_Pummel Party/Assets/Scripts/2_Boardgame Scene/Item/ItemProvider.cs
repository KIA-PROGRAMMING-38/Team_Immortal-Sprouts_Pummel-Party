using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public static class ItemProvider
{
    public static ItemData[] ItemTable = Resources.LoadAll<ItemData>("ItemData");
    
    private static int totalWeight;
    private static int TotalWeight
    {
        get
        {
            if(totalWeight != 0)
            {
                return totalWeight;
            }
            else
            {
                int weight = 0;

                foreach (ItemData item in ItemTable)
                {
                    weight += item.Weight;
                }

                totalWeight = weight;
                return totalWeight;
            }
        }

        set { totalWeight = value; }
    }
}
