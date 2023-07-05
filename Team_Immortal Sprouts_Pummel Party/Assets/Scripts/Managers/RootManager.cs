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
    public static DefaultPool PrefabPool;

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

        PrefabPool = PhotonNetwork.PrefabPool as DefaultPool;
    }


    private async void Start()
    {
        initCSV();
    }

    

    private void initCSV()
    {
        DataManager.Player.ReadCSV();
        DataManager.Item.ReadCSV();
        DataManager.MiniGame.ReadCSV();
    }
}
