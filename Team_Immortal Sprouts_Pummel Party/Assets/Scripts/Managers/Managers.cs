using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;

    private static ResourceManager _resourceManager = new ResourceManager();
    public static ResourceManager Resource { get { Init(); return _resourceManager; } }
    

    public static void Init()
    {
        if (s_instance is null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go is null)
            {
                go = new GameObject("@Managers");
                DontDestroyOnLoad(go);

                s_instance = go.AddComponent<Managers>();

                _resourceManager.Init();
            }
        }
        
    }
}
