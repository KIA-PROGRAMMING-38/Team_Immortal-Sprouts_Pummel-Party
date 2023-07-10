using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Controllable,
    UnControllable
}

public class ItemData : ILoadable
{
    public int ID { get; set; } 
    public string Name { get; set; }    
    public int ATK { get; set; }
    public string Description { get; set; } 
    public ItemType Type { get; set; }  
    public string FileName { get; set; }

    public void CacheResource()
    {
        Item item = Managers.Resource.Load<Item>(FileName);
        Managers.Resource.Items.Add(item);
    }
}

