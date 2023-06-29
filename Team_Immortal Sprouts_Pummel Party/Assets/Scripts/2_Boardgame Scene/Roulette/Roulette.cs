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
    
    public UnityEvent<int> OnRouletteFinished = new UnityEvent<int>();
    

    private void Awake()
    {
        playTokenSource = new CancellationTokenSource();
        cancelTokenSource = new CancellationTokenSource();
        cancelTokenSource.Cancel();
        targetSize = Vector3.one * 2f;
    }

    [SerializeField] [Range(0.5f, 2f)] private float appearTime = 1.5f;
    private Vector3 targetSize;
    private Vector3 disappearSize = Vector3.zero;
    private async void OnEnable()
    {
        await ExtensionMethod.SizeChange(transform, disappearSize, targetSize, appearTime); // 사이즈 조절
        rouletteRotate().Forget();
    }

    private bool isStoppable = false;
    private Vector3 rotationAxis = Vector3.up;
    [SerializeField] [Range(1f, 3f)] private float preheatTime = 3f;
    private async UniTaskVoid rouletteRotate()
    {
        await ExtensionMethod.DoRotate(roulette, 0f, rotateSpeed, rotationAxis, preheatTime);

        controlToken = playTokenSource.Token;
        isStoppable = true;
        
        while (true)
        {
            roulette.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            await UniTask.Yield(controlToken); // 토큰 넣어서 취소 가능하게끔 해주었음
        }
    }
    
    [SerializeField] [Range(3f, 5f)] private float slowTime = 3f;
    [SerializeField] [Range(1f, 3f)] private float changeTime = 1f;
    [SerializeField] [Range(0.5f, 1.5f)] private float shakeTime = 1f;
    [SerializeField] [Range(1f, 5f)] private float shakeIntensity = 3f;
    public async UniTaskVoid ShowDiceResult()
    {
        if (isStoppable == false)
        {
            return;
        }

        isStoppable = false;
        //rouletteResult = UnityEngine.Random.Range(MIN_RESULT, MAX_RESULT + 1); // 룰렛 값 추출
        rouletteResult = 5;
        controlToken = cancelTokenSource.Token; // 무한 돌기 멈춤

        ExtensionMethod.ShakeSpherePosition(transform, shakeTime, shakeIntensity).Forget();

        await ExtensionMethod.DoRotate(roulette, rotateSpeed, 0f, rotationAxis, slowTime); // 회전을 천천히 멈춤
        
        Quaternion initialRotation = roulette.rotation;
        Quaternion targetRotation = getRouletteRotation(rouletteResult); // 룰렛값에 해당하는 회전값을 가져옴

        await ExtensionMethod.QuaternionLerpExtension(roulette, initialRotation, targetRotation, changeTime); // 다시 값을 찾아서 회전함(약올리기)

        await UniTask.Delay(TimeSpan.FromSeconds(1f)); // 1초 기다렸다가
        await ExtensionMethod.SizeChange(transform, targetSize, disappearSize, appearTime);
        OnRouletteFinished?.Invoke(rouletteResult); // 룰렛이 끝나면 플레이어에게 룰렛값을 전달해줌
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
