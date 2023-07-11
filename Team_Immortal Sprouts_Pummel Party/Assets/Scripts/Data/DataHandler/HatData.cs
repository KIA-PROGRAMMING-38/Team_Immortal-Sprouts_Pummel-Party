using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatData : ILoadable
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string FileName { get; set; }

    public void CacheResource()
    {
        GameObject hat = Managers.Resource.Load<GameObject>(FileName);
        Managers.Resource.Hats.Add(hat);
    }
}