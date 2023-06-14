using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BeachPlayerController : MonoBehaviour
{
    public float playerSpeed;

    private Rigidbody playerBody;
    private float positionX;
    private float positionY;
    private float inputX;
    private float inputY;
    private bool isStart;
    private Vector3 moveVector;

    private void Awake()
    {
        if (Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
        }

        playerBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        isStart = false;
    }


    void Update()
    {
        if (!isStart)
        {
            InitSencer();

            isStart = true;
        }

        inputX = Accelerometer.current.acceleration.value.x - positionX;
        inputY = Accelerometer.current.acceleration.value.y - positionY;
        
        moveVector = new Vector3(inputX,0,inputY);
        playerBody.velocity = moveVector.normalized * playerSpeed;
    }

    private void InitSencer()
    {
        positionX = Accelerometer.current.acceleration.value.x;
        positionY = Accelerometer.current.acceleration.value.y;
    }
}
