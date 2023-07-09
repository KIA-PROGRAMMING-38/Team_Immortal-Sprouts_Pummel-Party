using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Item>
{
    Dictionary<Key, Item> MakeDic();
}

public class DataManager
{

    public void Init()
    {

    }
}
