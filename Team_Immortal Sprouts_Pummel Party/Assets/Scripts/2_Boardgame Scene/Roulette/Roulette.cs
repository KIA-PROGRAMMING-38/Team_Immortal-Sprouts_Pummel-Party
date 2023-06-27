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

    [SerializeField][Range(500f, 1000f)] private float rotateSpeed = 800f;
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

    [SerializeField] [Range(1f, 3f)] private float heatTime = 3f;
    private async UniTaskVoid rouletteRotate()
    {
        float elapsedTime = 0f;
        float currentSpeed = 0f;
        while (elapsedTime <= heatTime)
        {
            currentSpeed = ExtensionMethod.Lerp(0f, rotateSpeed, elapsedTime / heatTime);
            roulette.Rotate(Vector3.up, currentSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

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
        diceResult = UnityEngine.Random.Range(MIN_RESULT, MAX_RESULT + 1);
        controlToken = cancelTokenSource.Token;
        float elapsedTime = 0f;
        float currentSpeed = 0f;

        while (elapsedTime <= slowTime)
        {
            currentSpeed = ExtensionMethod.Lerp(rotateSpeed, 0f, elapsedTime / slowTime);
            roulette.Rotate(Vector3.up, currentSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        Quaternion initialRotation = roulette.rotation;
        Quaternion targetRotation = getDiceRotation(diceResult);

        ExtensionMethod.QuaternionLerpExtension(roulette, initialRotation, targetRotation, changeTime).Forget();
    }

    private Quaternion getDiceRotation(int diceResult) => rotationValues[diceResult + 1];

    private Quaternion[] rotationValues = new Quaternion[]
    {
        Quaternion.Euler(141f, 0f, 90f), // 주사위값이 -1 이 나왔을때의 회전값
        Quaternion.Euler(195f, 0f, 90f), // 주사위값이 0 이 나왔을때의 회전값
        Quaternion.Euler(350f, 0f, 90f), // 주사위값이 1 이 나왔을때의 회전값
        Quaternion.Euler(40f, 0f, 90f), // 주사위값이 2 이 나왔을때의 회전값
        Quaternion.Euler(92f, 0f, 90f), // 주사위값이 3 이 나왔을때의 회전값
        Quaternion.Euler(246f, 0f, 90f), // 주사위값이 4 이 나왔을때의 회전값
        Quaternion.Euler(298f, 0f, 90f), // 주사위값이 5 이 나왔을때의 회전값
    };
}
