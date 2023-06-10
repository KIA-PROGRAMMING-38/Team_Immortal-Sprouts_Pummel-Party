using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody planeBody;
    private Vector3 movePosition;
    [SerializeField] private GameObject playerCamera;
    private CinemachineVirtualCamera virtualCamera;
  
    

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
        
        RotatePlane().Forget();
    }


    private bool isStart;
    private Vector3 controllVector;
    private float positionX;
    private float positionY;
    [SerializeField] private float speed;

    private void Update()
    {
        if (!isStart) 
        {
            InitSencer();

            isStart = true;
        }

        controllVector = new Vector3((Accelerometer.current.acceleration.value.x - positionX), -(Accelerometer.current.acceleration.value.y - positionY), 0);

        planeBody.velocity = (movePosition + controllVector).normalized * speed;

        if (virtualCamera.m_Lens.Dutch >= 20)
        {
            virtualCamera.m_Lens.Dutch = 20;
        }

        if (virtualCamera.m_Lens.Dutch <= -20)
        {
            virtualCamera.m_Lens.Dutch = -20;
        }
    }

    private async UniTaskVoid RotatePlane()
    {
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            if (Accelerometer.current.acceleration.value.x - positionX > 0.2 || Accelerometer.current.acceleration.value.x - positionX < -0.2)
            {
                virtualCamera.m_Lens.Dutch = (Accelerometer.current.acceleration.value.x - positionX) * 10;
            }
            else
            {
                virtualCamera.m_Lens.Dutch = 0;
            }
        }
    }

    public void InitSencer()
    {
        positionX = Accelerometer.current.acceleration.value.x;
        positionY = Accelerometer.current.acceleration.value.y;
    }
}
