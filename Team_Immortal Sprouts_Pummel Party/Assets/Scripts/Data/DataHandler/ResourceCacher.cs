using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceCacher<T> where T : Object
{
    private Dictionary<string, T> cache = new();
    
    public T LoadResource(string fileName)
    {
        if (!cache.ContainsKey(fileName))
        {
            T resource = Managers.Resource.Load<T>(fileName);
            cache[fileName] = resource;

            return resource;
        }
        else
        {
            return cache[fileName];
        }
    }

    public T GetResource(string fileName) => cache[fileName];

}
