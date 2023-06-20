using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class SummonCloud : MonoBehaviour
{
    [SerializeField] [Range(0.5f, 4f)] private float flyTime = 2f;
    [SerializeField] [Range(0.5f, 3f)] private float sizeTime = 1.5f;
    
    private Vector3 disappearSize = Vector3.zero;   
    private Vector3 initialSize = Vector3.one;
    private Vector3 bigSize = Vector3.one * 2f; // 2가 되었을 때 플레이어도 쏙 가리고, 괜찮은듯 함?

    [SerializeField] Transform playerTransform;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CallCloud();
        }
    }

    public async void CallCloud()
    {
        await FlyToSummonSpot(playerTransform.position + Vector3.up, flyTime);
        CloudResize(true);
    }


    public void CloudResize(bool shouldGetBigger)
    {
        if (shouldGetBigger)
        {
            controlSize(initialSize, bigSize, sizeTime).Forget();
        }
        else
        {
            controlSize(bigSize, disappearSize, sizeTime).Forget();
        }
    }

    private async UniTask FlyToSummonSpot(Vector3 summonPosition, float flyTime)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 midPoint = Vector3.Lerp(initialPosition, summonPosition, 0.5f);
        float magnitude = Vector3.Magnitude(midPoint - summonPosition);
        Vector3 controlPoint = midPoint - Vector3.up * magnitude;

        while (elapsedTime <= flyTime)
        {
            float t = elapsedTime / flyTime;
            Vector3 m1 = Vector3.Lerp(initialPosition, controlPoint, t);
            Vector3 m2 = Vector3.Lerp(controlPoint, summonPosition, t);

            transform.position = Vector3.Lerp(m1, m2, t);

            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

    }

    private async UniTask controlSize(Vector3 startSize, Vector3 endSize, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            transform.localScale = Vector3.Lerp(startSize, endSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

}
