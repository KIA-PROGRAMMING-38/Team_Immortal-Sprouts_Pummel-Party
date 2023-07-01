using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TreeEditor;
using UnityEngine;

public class AwardLightController : MonoBehaviour
{
    [SerializeField] [Range(50f, 150f)] private float moveSpeed = 80f;
    [SerializeField] [Range(0.3f, 1f)] private float winnerReachTime = 0.5f;
    [SerializeField] [Range(45f, 60f)] private float minXRotation = 45f;
    [SerializeField] [Range(110f, 135f)] private float maxXRotation = 135f;


    private const float MIN_Y_ROTATION = 0f;
    private const float MAX_Y_ROTATION = 359f;

    private CancellationTokenSource playSource;
    private CancellationTokenSource stopSource;
    private CancellationToken token;


    public Transform winnerTransform;

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
            if (testWinnerShowing)
            {
                testWinnerShowing= false;
                lightRandomMove().Forget();
            }
            else
            {
                testWinnerShowing = true;
                Vector3 winnerPosition = winnerTransform.position;
                LightWinner(winnerPosition - transform.position).Forget();
            }
            
        }
    }


    private bool testWinnerShowing = false;

    public async UniTask LightWinner(Vector3 lookDirection)
    {
        stopRandomMovement();
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        ExtensionMethod.QuaternionLerpExtension(transform, initialRotation, targetRotation, winnerReachTime).Forget();
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
