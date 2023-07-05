using Cysharp.Threading.Tasks;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    private static RootManager instance = null;
    public static RootManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public static FrameWorkManager FrameWorkManager = new FrameWorkManager();
    public static TurnManager BoardGameManager = new TurnManager();
    public static LoadManager LoadManager = new LoadManager();  
    public static DataManager DataManager = new DataManager();
    public static DefaultPool PrefabManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        PrefabManager = PhotonNetwork.PrefabPool as DefaultPool;
    }


    private void Start()
    {
        DataManager.InitCSV();
    }
}
