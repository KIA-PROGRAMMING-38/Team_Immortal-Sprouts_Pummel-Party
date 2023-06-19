using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ScanBall : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 1f)] private float activateGapTime = 1f;
    [SerializeField] [Range(0.1f, 1.5f)] private float scanTime = 1f;
    [SerializeField] [Range(1f, 3f)] private float waitTime = 2f;
    [SerializeField] [Range(0.1f, 1f)] private float minSize = 0.1f;
    [SerializeField] [Range(30f, 40f)] private float maxSize = 30f;

    private void Start()
    {
        ActivateScanning(activateGapTime).Forget();
    }

    public async UniTaskVoid ActivateScanning(float waitTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
        startScanLoop().Forget();
    }

    private async UniTaskVoid startScanLoop()
    {
        await sizeLerp(scanTime, minSize, maxSize);
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
        await sizeLerp(scanTime, maxSize, minSize);
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
        startScanLoop().Forget();
    }


    private async UniTask<int> sizeLerp(float targetTime, float startSize, float targetSize)
    {
        float elapsedTime = 0f;
        float size = 0f;
        Vector3 sizeVector = Vector3.zero;
        while (elapsedTime <= targetTime)
        {
            size = Mathf.Lerp(startSize, targetSize, elapsedTime / targetTime);
            sizeVector.Set(size, size, size);
            transform.localScale = sizeVector;
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        return 0;
    }
}
