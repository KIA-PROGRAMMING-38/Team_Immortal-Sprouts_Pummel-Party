using System.Collections.Generic;
using UnityEngine;



public class ResourceManager
{
    public void Init()
    {
        ItemTable = CacheResource<Item, ItemData>(Managers.Data.Items);
        BodyMatTable = CacheResource<Material, BodyData>(Managers.Data.Bodies);
        HatTable = CacheResource<GameObject, HatData>(Managers.Data.Hats);
    }

    public ResourceCacher<Material> BodyMatTable { get; private set; } 
    public ResourceCacher<GameObject> HatTable { get; private set; }
    public ResourceCacher<Item> ItemTable { get; private set; }

    // TODO
    // - 추후 자주 로드되는 리소스는 캐싱이 필요함.
    // - 또한, Resources API가 아닌 AssetBundle을 사용해야 할 경우 로직이 변경될 수 있음.

    private ResourceCacher<T> CacheResource<T, T1>(List<T1> dataList) where T : Object where T1 : ILoadble
    {
        ResourceCacher<T> cache = new ResourceCacher<T>();

        foreach (T1 element in dataList)
        {
            cache.LoadResource(element.GetFileName());
        }

        return cache;
    }

    public T Load<T>(string filename) where T : UnityEngine.Object
    {
        return Resources.Load<T>(filename);
    }

    public T Instantiate<T>(string filename) where T : UnityEngine.Object
    {
        var resource = Load<T>(filename);

        return UnityEngine.Object.Instantiate(resource);
    }
    
    public void Destroy(Object obj)
    {
        UnityEngine.Object.Destroy(obj);
    }
}
