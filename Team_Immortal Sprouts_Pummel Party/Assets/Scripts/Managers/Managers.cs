using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;

    private static ResourceManager _resourceManager = new ResourceManager();
    public static ResourceManager Resource { get { InitManagers(); return _resourceManager; } }

    private static DataManager _dataManager = new DataManager();
    public static DataManager Data { get { InitManagers(); return _dataManager; } }

    private static PhotonManager _photonManager = null;
    public static PhotonManager Photon { get { InitManagers(); return _photonManager; } }

    private static UIManager _uiManager = new UIManager();
    public static UIManager UI { get { InitManagers(); return _uiManager; } }   

    public static void InitManagers()
    {
        if (s_instance is null)
        {
            GameObject go = GameObject.Find("@Managers");

            if (go is null)
            {
                go = new GameObject("@Managers");
                DontDestroyOnLoad(go);
            }

            s_instance = go.AddComponent<Managers>();
            _photonManager = CreateComponentManager<PhotonManager>();

            _uiManager.Init();
            _dataManager.Init();
            _resourceManager.Init();
            _photonManager.Init();

        }

        
    }

    public static T CreateComponentManager<T>() where T : UnityEngine.Component
    {
        GameObject managerGO = new GameObject($"{typeof(T)}");
        managerGO.transform.SetParent(s_instance.transform);
        T instance = managerGO.AddComponent<T>();

        return instance;
    }
}
