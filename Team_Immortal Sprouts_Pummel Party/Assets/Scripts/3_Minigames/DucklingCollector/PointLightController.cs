using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PointLightController : MonoBehaviour
{
    [SerializeField] private Light pointLight;

    [Header("------------------------------Controllable------------------------------")]
    [SerializeField][Range(0.5f, 1f)] private float minRange = 1f;
    [SerializeField][Range(1.5f, 3f)] private float normalRange = 3f;
    [SerializeField][Range(35f, 40f)] private float maxRange = 35f;

    [SerializeField] private int repeatTime = 3;
    [SerializeField][Range(1f, 3f)] private float normalTime = 1f;
    [SerializeField][Range(0.2f, 1f)] private float maxShowTime = 0.5f;
    [SerializeField][Range(0.1f, 1f)] private float maxReachTime = 0.5f;

    private void Start()
    {
        enlighten().Forget();
    }

    private async UniTaskVoid enlighten()
    {
        int repeatedTime = 0;
        while (repeatedTime < repeatTime)
        {
            await pointLightLerp(normalTime, minRange, normalRange);

            await pointLightLerp(normalTime, normalRange, minRange);

            ++repeatedTime;
        }

        await pointLightLerp(maxReachTime, minRange, maxRange);

        await UniTask.Delay(TimeSpan.FromSeconds(maxShowTime));

        await pointLightLerp(maxReachTime, maxRange, minRange);

        enlighten().Forget();
    }

    private async UniTask pointLightLerp(float targetTime, float startRange, float targetRange)
    {
        float elapsedTime = 0f;
        while (elapsedTime <= targetTime)
        {
            pointLight.range = Mathf.Lerp(startRange, targetRange, elapsedTime / targetTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

}
