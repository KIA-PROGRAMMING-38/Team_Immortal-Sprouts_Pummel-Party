using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TreeEditor;
using UnityEngine;

public class AwardLightController : MonoBehaviour
{
    [SerializeField] [Range(50f, 150f)] private float moveSpeed = 80f;
    [SerializeField] private float minXRotation = 45f;
    [SerializeField] private float maxXRotation = 135f;


    private const float MIN_Y_ROTATION = 0f;
    private const float MAX_Y_ROTATION = 359f;

    private CancellationTokenSource playSource;
    private CancellationTokenSource stopSource;
    private CancellationToken token;

    private void Awake()
    {
        playSource = new CancellationTokenSource();
        stopSource = new CancellationTokenSource(); 
        stopSource.Cancel();
        token = playSource.Token;
    }

    private void OnEnable()
    {
        lightRandomMove().Forget();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stopRandomMovement();
        }
    }

    private async UniTask lightRandomMove()
    {
        token= playSource.Token;
        Quaternion targetRotation = getRandomRotation();

        while (3f <= Quaternion.Angle(transform.rotation, targetRotation))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
            await UniTask.Yield(token);
        }

        lightRandomMove().Forget();
    }

    private Quaternion getRandomRotation()
    {
        Quaternion targetRotation;
        float xValue = Random.Range(minXRotation, maxXRotation);
        float yValue = Random.Range(MIN_Y_ROTATION, MAX_Y_ROTATION);
        targetRotation = Quaternion.Euler(xValue, yValue, 0f);

        return targetRotation;
    }

    private void stopRandomMovement() => token = stopSource.Token;  
    
}
