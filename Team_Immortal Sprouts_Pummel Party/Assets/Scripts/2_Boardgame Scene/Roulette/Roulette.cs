using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    [SerializeField] private Transform roulette;

    [SerializeField][Range(800f, 1500f)] private float rotateSpeed = 1200f;
    private const int MIN_RESULT = -1;
    private const int MAX_RESULT = 5;
    private int diceResult; // 주사위값
    private CancellationTokenSource playTokenSource;
    private CancellationTokenSource cancelTokenSource;
    private CancellationToken controlToken;

    private void Awake()
    {
        playTokenSource = new CancellationTokenSource();
        cancelTokenSource = new CancellationTokenSource();
        cancelTokenSource.Cancel();
    }

    private void OnEnable()
    {
        rouletteRotate().Forget();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            showDiceResult().Forget();
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
        diceResult = UnityEngine.Random.Range(MIN_RESULT, MAX_RESULT + 1); // 룰렛 값 추출
        controlToken = cancelTokenSource.Token; // 무한 돌기 멈춤

        await ExtensionMethod.DoRotate(roulette, rotateSpeed, 0f, rotationAxis, slowTime);
        
        Quaternion initialRotation = roulette.rotation;
        Quaternion targetRotation = getRouletteRotation(diceResult); // 룰렛값에 해당하는 회전값을 가져옴

        ExtensionMethod.QuaternionLerpExtension(roulette, initialRotation, targetRotation, changeTime).Forget();
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
