using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;



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

        foreach (T1 data in dataList)
        {
            cache.LoadResource(data.GetFileName());
        }

        return cache;
    }

    public T Load<T>(string filename) where T : Object => Resources.Load<T>(filename);

    public T Instantiate<T>(string filename) where T : Object
    {
        var resource = Load<T>(filename);
        return Object.Instantiate(resource);
    }

    public void Destroy(Object obj)
    {
        if (obj == null) { return; }

        Object.Destroy(obj);
    }
}
