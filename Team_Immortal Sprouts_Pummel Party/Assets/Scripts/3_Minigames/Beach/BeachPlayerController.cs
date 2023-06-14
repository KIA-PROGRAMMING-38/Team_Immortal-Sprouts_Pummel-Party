using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeachPlayerController : MonoBehaviour
{
    private Rigidbody playerBody;

    private void Awake()
    {
        if (Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
        }

        playerBody = playerBody.GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
