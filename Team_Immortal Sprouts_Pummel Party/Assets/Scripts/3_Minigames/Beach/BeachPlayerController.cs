using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BeachPlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
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
        moveVector = new Vector3();
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
        moveVector.Set(inputX, 0, inputY);

        playerBody.velocity = moveVector.normalized * playerSpeed;
    }

    private void InitSencer()
    {
        positionX = Accelerometer.current.acceleration.value.x;
        positionY = Accelerometer.current.acceleration.value.y;
    }
    #region Indexing
    [SerializeField] ParticleSystem[] effects;
    private const int EXPLOSION = 0;
    private const int BIG_WATER_SPLASH = 1;
    private const int SMALL_WATER_SPLASH = 2;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Bullet")
        {
            return;
        }

        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].gameObject.transform.SetParent(null);
        }

        gameObject.SetActive(false);
        effects[EXPLOSION].Play();
        PlayWaterEffect().Forget();
    }

    private async UniTaskVoid PlayWaterEffect()
    {
        await UniTask.Delay(1000);
        effects[BIG_WATER_SPLASH].Play();

        for (int i = SMALL_WATER_SPLASH; i < effects.Length; i++)
        {
            await UniTask.Delay(200);
            effects[i].Play();
        }
    }
}
