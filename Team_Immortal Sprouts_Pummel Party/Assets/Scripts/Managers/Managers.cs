using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance = null;
    public static Managers Instance
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
    }


    private void Start()
    {
        
    }

    
}
