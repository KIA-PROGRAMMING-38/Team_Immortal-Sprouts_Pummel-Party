using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BodyData :ILoadable
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string FileName { get; set; }

    public void CacheResource()
    {
        Material material = Managers.Resource.Load<Material>(FileName);
        Managers.Resource.BodyMaterial.Add(material);
    }
}
