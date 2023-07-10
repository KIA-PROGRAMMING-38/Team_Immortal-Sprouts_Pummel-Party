using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AwardLightController : MonoBehaviour
{
    [SerializeField] private AwardProvider awardProvider;

    [Header("---------------------------Controllable---------------------------")]
    [SerializeField][Range(1f, 5f)] private float intensityControlTime = 2.5f;
    [SerializeField][Range(50f, 150f)] private float moveSpeed = 80f;
    [SerializeField][Range(30f, 50f)] private float minXRotation = 45f;
    [SerializeField][Range(80f, 95f)] private float maxXRotation = 90f;
    [SerializeField][Range(0.3f, 1f)] private float winnerReachTime = 0.5f;


    private const float MIN_Y_ROTATION = -90f;
    private const float MAX_Y_ROTATION = 90f;

    #region 토큰
    private CancellationTokenSource playSource;
    private CancellationTokenSource stopSource;
    private CancellationToken token;
    #endregion

    #region 불빛 세기
    private Light spotLight;
    [SerializeField] private float initialIntensity = 30f;

    private Material lightRangeMaterial;
    [SerializeField] private float initialLightRangeOpacity = 0.02f;
    private const string OPACITY_KEY = "_Opacity";
    #endregion


    private void Awake()
    {
        init();
    }

    private async void OnEnable()
    {
        //awardProvider.OnGiveAward.RemoveListener(lightWinner);
        //awardProvider.OnAwardGiven.RemoveListener(activateLightRandomMove);
        //awardProvider.OnGiveAward.AddListener(lightWinner);
        //awardProvider.OnAwardGiven.AddListener(activateLightRandomMove);
        //await gradualLightIntensityControl();
        //activateLightRandomMove();
    }

    private void OnDisable()
    {
        //awardProvider.OnGiveAward.RemoveListener(lightWinner);
        //awardProvider.OnAwardGiven.RemoveListener(activateLightRandomMove);
    }


    public float GetReachTime() => winnerReachTime;

    private void init()
    {
        playSource = new CancellationTokenSource();
        stopSource = new CancellationTokenSource();
        stopSource.Cancel();
        token = playSource.Token;
        spotLight = GetComponent<Light>();
        lightRangeMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }

    [SerializeField] private float waitTime = 2f;
    private async UniTask gradualLightIntensityControl()
    {
        spotLight.intensity = 0f;
        lightRangeMaterial.SetFloat(OPACITY_KEY, 0f);

        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));

        float elapsedTime = 0f; // 나중에 ExtensionMethod에 추가해야할듯
        while (elapsedTime <= intensityControlTime)
        {
            spotLight.intensity = Lerp(0f, initialIntensity, elapsedTime / intensityControlTime);
            lightRangeMaterial.SetFloat(OPACITY_KEY, Lerp(0f, initialLightRangeOpacity, elapsedTime / intensityControlTime));
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private void activateLightRandomMove() => lightRandomMove().Forget();

    private async UniTask lightRandomMove()
    {
        token = playSource.Token;

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
        float xValue = UnityEngine.Random.Range(minXRotation, maxXRotation);
        float yValue = UnityEngine.Random.Range(MIN_Y_ROTATION, MAX_Y_ROTATION);
        targetRotation = Quaternion.Euler(xValue, yValue, 0f);

        return targetRotation;
    }

    private void lightWinner(Transform winnerTransform)
    {
        stopRandomMovement();
        Vector3 winnerDirection = winnerTransform.position - transform.position;

        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(winnerDirection);

        ExtensionMethod.QuaternionLerpExtension(transform, initialRotation, targetRotation, winnerReachTime).Forget();
    }

    private void stopRandomMovement() => token = stopSource.Token;

    private float Lerp(float start, float end, float t)
    {
        return start + (end - start) * t;
    }

}
