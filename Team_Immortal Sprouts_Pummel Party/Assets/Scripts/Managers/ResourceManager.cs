using JetBrains.Annotations;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    #region 자주 로드되는 리소스 캐싱 리스트
    public List<Material> BodyMaterial { get; private set; } = new List<Material>();
    public List<GameObject> Hats { get; private set; } = new List<GameObject>();
    public List<Item> Items { get; private set; } = new List<Item>();

    #endregion

    public void Init()
    {

    }


    // TODO
    // - 추후 자주 로드되는 리소스는 캐싱이 필요함.
    // - 또한, Resources API가 아닌 AssetBundle을 사용해야 할 경우 로직이 변경될 수 있음.
    public T Load<T>(string filename) where T : UnityEngine.Object => Resources.Load<T>(filename);


    public T Instantiate<T>(string filename) where T : UnityEngine.Object
    {
        var resource = Load<T>(filename);

        return UnityEngine.Object.Instantiate(resource);
    }

    public void Destroy(Object obj) => UnityEngine.Object.Destroy(obj);



    public GameObject PhotonInstantiate(string filename, Vector3 position, Quaternion rotation)
    {
        return PhotonNetwork.Instantiate(filename, position, rotation);
    }

    public void PhotonDestory([NotNull]GameObject go = null, PhotonView pv = null)
    {
        if (go != null)
        {
            PhotonNetwork.Destroy(go);
        }
        else if (pv != null)
        {
            PhotonNetwork.Destroy(pv);
        }
    }
}
