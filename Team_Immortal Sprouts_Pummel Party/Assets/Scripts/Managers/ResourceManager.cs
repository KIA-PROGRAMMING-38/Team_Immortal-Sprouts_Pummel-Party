using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public List<Material> BodyMaterials { get; private set; } = new List<Material>();
    public List<GameObject> Hats { get; private set; } = new List<GameObject>();
    public List<Item> Items { get; private set; } = new List<Item>(); 
    public void Init()
    {

    }

    // TODO
    // - 추후 자주 로드되는 리소스는 캐싱이 필요함.
    // - 또한, Resources API가 아닌 AssetBundle을 사용해야 할 경우 로직이 변경될 수 있음.
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
