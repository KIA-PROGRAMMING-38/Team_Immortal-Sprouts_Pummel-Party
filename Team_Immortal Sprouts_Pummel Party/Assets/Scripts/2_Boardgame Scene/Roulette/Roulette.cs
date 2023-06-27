using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Roulette : MonoBehaviour
{
    [SerializeField] private Transform roulette;
    [SerializeField][Range(800f, 1500f)] private float rotateSpeed = 1200f;
    private const int MIN_RESULT = -1;
    private const int MAX_RESULT = 5;
    private int rouletteResult; // 룰렛값
    private CancellationTokenSource playTokenSource;
    private CancellationTokenSource cancelTokenSource;
    private CancellationToken controlToken;

    public UnityEvent<int> OnStoppedRoulette = new UnityEvent<int>();
    

    private void Awake()
    {
        playTokenSource = new CancellationTokenSource();
        cancelTokenSource = new CancellationTokenSource();
        cancelTokenSource.Cancel();
        targetSize = transform.localScale;
    }

    [SerializeField] [Range(0.5f, 2f)] private float appearTime = 1.5f;
    private Vector3 targetSize;
    private Vector3 disappearSize = Vector3.zero;
    private async void OnEnable()
    {
        await ExtensionMethod.SizeChange(transform, disappearSize, targetSize, appearTime);
        rouletteRotate().Forget();
    }

    


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            showDiceResult().Forget();
        }
    }

    public void Test(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Touch함");
        }
    }

    private Vector3 rotationAxis = Vector3.up;
    [SerializeField] [Range(1f, 3f)] private float preheatTime = 3f;
    private async UniTaskVoid rouletteRotate()
    {
        await ExtensionMethod.DoRotate(roulette, 0f, rotateSpeed, rotationAxis, preheatTime);

        controlToken = playTokenSource.Token;

        while (true)
        {
            roulette.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            await UniTask.Yield(controlToken);
        }
    }
    
    [SerializeField] [Range(3f, 5f)] private float slowTime = 3f;
    [SerializeField] [Range(1f, 3f)] private float changeTime = 1f;
    private async UniTaskVoid showDiceResult()
    {
        rouletteResult = UnityEngine.Random.Range(MIN_RESULT, MAX_RESULT + 1); // 룰렛 값 추출
        controlToken = cancelTokenSource.Token; // 무한 돌기 멈춤

        await ExtensionMethod.DoRotate(roulette, rotateSpeed, 0f, rotationAxis, slowTime);
        
        Quaternion initialRotation = roulette.rotation;
        Quaternion targetRotation = getRouletteRotation(rouletteResult); // 룰렛값에 해당하는 회전값을 가져옴

        await ExtensionMethod.QuaternionLerpExtension(roulette, initialRotation, targetRotation, changeTime);

        await UniTask.Delay(TimeSpan.FromSeconds(1f)); // 1초 기다렸다가
        await ExtensionMethod.SizeChange(transform, targetSize, disappearSize, appearTime);
        OnStoppedRoulette?.Invoke(rouletteResult);
    }

    private Quaternion getRouletteRotation(int diceResult) => rotationValues[diceResult + 1];

    private readonly Quaternion[] rotationValues = new Quaternion[]
    {
        Quaternion.Euler(141f, 0f, 90f), // 룰렛값이 -1 이 나왔을때의 회전값
        Quaternion.Euler(195f, 0f, 90f), // 룰렛값이 0 이 나왔을때의 회전값
        Quaternion.Euler(350f, 0f, 90f), // 룰렛값이 1 이 나왔을때의 회전값
        Quaternion.Euler(40f, 0f, 90f), // 룰렛값이 2 이 나왔을때의 회전값
        Quaternion.Euler(92f, 0f, 90f), // 룰렛값이 3 이 나왔을때의 회전값
        Quaternion.Euler(246f, 0f, 90f), // 룰렛값이 4 이 나왔을때의 회전값
        Quaternion.Euler(298f, 0f, 90f), // 룰렛값이 5 이 나왔을때의 회전값
    };
}
