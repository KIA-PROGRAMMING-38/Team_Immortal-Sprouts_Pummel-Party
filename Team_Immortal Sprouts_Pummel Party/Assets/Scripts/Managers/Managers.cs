using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{

    public static FrameWorkManager FrameWorkManager = new FrameWorkManager();
    public static BoardGameManager BoardGameManager = new BoardGameManager();
    public static LoadManager LoadManager = new LoadManager();  
    public static DataManager DataManager = new DataManager();

    private void Awake()
    {
        
    }


    private void Start()
    {
        
    }

    
}
