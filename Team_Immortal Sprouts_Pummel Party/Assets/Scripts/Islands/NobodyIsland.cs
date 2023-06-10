using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NobodyIsland : Island
{
    [SerializeField] private Transform[] currentPlayerSpots;
    void Start()
    {
        InitPositionSettings().Forget();
        SetCurrentPosition(currentPlayerSpots[0].position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
