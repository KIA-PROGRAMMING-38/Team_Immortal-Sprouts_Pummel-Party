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
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject boostObj;
    private ParticleSystem boostEffect;
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
        explosion = explosionObj.GetComponent<ParticleSystem>();
        boostEffect = boostObj.GetComponent<ParticleSystem>();


        if (Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
        }
    }

    private void Start()
    {
        upVector = -90f;
        downVector = 90;
        rightYVector = 90;
        rightZVector = -90;
        leftYVector = -90;
        leftZVector = 90;
        smoothFactor = 0.1f;

    }

    #region move

    private bool isStart;
    private Quaternion controllVector;
    private float positionX;
    private float positionY;
    [SerializeField] private float speed;
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

        smoothAngleY = Mathf.Lerp(upVector, downVector, (Accelerometer.current.acceleration.value.y - positionY + 1) / 2f);
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
    #endregion


    #region collision

    [SerializeField] private GameObject explosionObj;
    private ParticleSystem explosion;
    [SerializeField] private Transform spawnPosition;

    private void OnCollisionEnter()
    {
        gameObject.SetActive(false);
        speed = 0;
        explosionObj.transform.position = transform.position;
        explosion.Play();

        Spawn().Forget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Area"))
        {
            return;
        }
        Debug.Log("Ãæµ¹");
        boostEffect.Play();
        speed += 10;
    }

    private async UniTaskVoid Spawn()
    {
        await UniTask.Delay(3000);
        transform.position = spawnPosition.position;
        speed = 60;
        explosion.Stop();
        InitSencer();
        gameObject.SetActive(true);
    }
    #endregion

}
