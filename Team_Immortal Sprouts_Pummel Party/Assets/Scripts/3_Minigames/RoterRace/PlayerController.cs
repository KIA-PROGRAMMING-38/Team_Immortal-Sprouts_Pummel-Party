using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody planeBody;
    private Vector3 movePosition;
    [SerializeField] private GameObject playerCamera;
    private CinemachineVirtualCamera virtualCamera;
    private float upVector;
    private float downVector;
    private float leftYVector;
    private float leftZVector;
    private float rightYVector;
    private float rightZVector;
    private float smoothFactor;

    private void Awake()
    {
        planeBody = GetComponent<Rigidbody>();
        virtualCamera = playerCamera.GetComponent<CinemachineVirtualCamera>();

        if (Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
        }
        movePosition = new Vector3(0.0f, 0.0f, 1.0f);
    }

    private void Start()
    {
        upVector = -90f;
        downVector = 90;
        rightYVector = 45;
        rightZVector = -45;
        leftYVector = -45;
        leftZVector = 45;
        smoothFactor = 0.1f;

    }


    private bool isStart;
    private Quaternion controllVector;
    private float positionX;
    private float positionY;
    [SerializeField] private float speed;
    [SerializeField] private float angleSpeed;
    private float smoothAngleY;
    private float smoothAngleX;
    private float smoothAngleZ;



    private void Update()
    {
        if (!isStart)
        {
            InitSencer();

            isStart = true;
        }




        planeBody.velocity = transform.forward * speed;

        smoothAngleY = Mathf.Lerp(downVector, upVector, (Accelerometer.current.acceleration.value.y - positionY + 1) / 2f);
        smoothAngleX = Mathf.Lerp(leftYVector, rightYVector, (Accelerometer.current.acceleration.value.x - positionX + 1) / 2f);
        smoothAngleZ = Mathf.Lerp(leftZVector, rightZVector, (Accelerometer.current.acceleration.value.x - positionX + 1) / 2f);

        controllVector = Quaternion.Euler(smoothAngleY, smoothAngleX, smoothAngleZ);


        transform.rotation = Quaternion.Slerp(transform.rotation, controllVector, smoothFactor);
    }


    public void InitSencer()
    {
        positionX = Accelerometer.current.acceleration.value.x;
        positionY = Accelerometer.current.acceleration.value.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Area"))
        {
            return;
        }
        speed += 10;
    }
}
