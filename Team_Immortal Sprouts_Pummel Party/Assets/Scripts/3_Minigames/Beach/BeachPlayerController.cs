using Cysharp.Threading.Tasks;
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

        moveVector = new Vector3(inputX, 0, inputY);
        inputX = Accelerometer.current.acceleration.value.x - positionX;
        inputY = Accelerometer.current.acceleration.value.y - positionY;

        playerBody.velocity = moveVector.normalized * playerSpeed;
    }

    private void InitSencer()
    {
        positionX = Accelerometer.current.acceleration.value.x;
        positionY = Accelerometer.current.acceleration.value.y;
    }
    #region Indexing
    [SerializeField] ParticleSystem[] effect;
    private const int explosion = 0;
    private const int bigWaterSplash = 1;
    private const int smallWaterSplash1 = 2;
    private const int smallWaterSplash2 = 3;
    private const int smallWaterSplash3 = 4;
    private const int waterRipple1 = 5;
    private const int waterRipple2 = 6;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Bullet")
        {
            return;
        }

        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].gameObject.transform.SetParent(null);
        }

        gameObject.SetActive(false);
        effect[explosion].Play();
        PlayWaterEffect().Forget();
    }

    private async UniTaskVoid PlayWaterEffect()
    {
        await UniTask.Delay(1000);
        effect[bigWaterSplash].Play();

        for (int i = smallWaterSplash1; i < effect.Length; i++)
        {
            await UniTask.Delay(200);
            effect[i].Play();
        }
    }
}
