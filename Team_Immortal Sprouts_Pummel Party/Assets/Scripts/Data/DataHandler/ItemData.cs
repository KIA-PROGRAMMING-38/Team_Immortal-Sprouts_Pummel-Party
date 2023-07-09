using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Controllable,
    UnControllable
}

public class ItemData 
{
    public int ID { get; set; } 
    public string Name { get; set; }    
    public int ATK { get; set; }
    public string Description { get; set; } 
    public ItemType Type { get; set; }  
}

public class ItemDataLoader : ILoader<int, ItemData>
{
    public List<ItemData> _itemData = new List<ItemData>();

    public Dictionary<int, ItemData> MakeDic()
    {
        Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();

        foreach (ItemData data in _itemData)
        {
            dict.Add(data.ID, data);
        }

        return dict;
    }
}
