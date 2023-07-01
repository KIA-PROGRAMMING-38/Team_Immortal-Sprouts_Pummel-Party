using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TreeEditor;
using UnityEngine;

public class AwardLightController : MonoBehaviour
{
    [SerializeField] private AwardProvider awardProvider;

    [Header("---------------------------Controllable---------------------------")]
    [SerializeField] [Range(50f, 150f)] private float moveSpeed = 80f;
    [SerializeField] [Range(30f, 50f)] private float minXRotation = 45f;
    [SerializeField] [Range(80f, 95f)] private float maxXRotation = 90f;
    [SerializeField] [Range(0.3f, 1f)] private float winnerReachTime = 0.5f;


    private const float MIN_Y_ROTATION = -90f;
    private const float MAX_Y_ROTATION = 90f;

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
        awardProvider.OnGiveAward.RemoveListener(lightWinner);
        awardProvider.OnGiveAward.AddListener(lightWinner);
        lightRandomMove().Forget();
    }

    private void OnDisable()
    {
        awardProvider.OnGiveAward.RemoveListener(lightWinner);
    }


    private void lightWinner(Transform winnerTransform)
    {
        stopRandomMovement();
        Vector3 winnerDirection = winnerTransform.position - transform.position;  

        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(winnerDirection);

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

    public float GetReachTime() => winnerReachTime;
    
}
