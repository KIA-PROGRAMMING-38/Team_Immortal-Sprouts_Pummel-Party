using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NobodyIsland : Island
{
    [SerializeField] Transform[] nobodyIslandPlayerSpots;
    
    void Start()
    {
        InitPositionSettings().Forget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
