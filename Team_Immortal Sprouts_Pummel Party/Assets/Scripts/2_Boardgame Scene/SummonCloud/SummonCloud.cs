using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class SummonCloud : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cloudBody;
    [SerializeField] [Range(0.5f, 4f)] private float flyTime = 2f;
    [SerializeField] [Range(0.5f, 3f)] private float sizeTime = 1.5f;
    [SerializeField] [Range(1f, 3f)] private float gapDistance = 2f;
    [SerializeField] private Light spotLight;
    
    private Vector3 disappearSize = Vector3.zero;   
    private Vector3 initialSize = Vector3.one;
    private Vector3 bigSize = Vector3.one * 2f; // 2가 되었을 때 플레이어도 쏙 가리고, 괜찮은듯 함?
    private const float pivotModifier = 0.8f; // 구름 피봇이 중앙이 아니라서 이걸로 플레이어 중앙에 맞출 수 있음
    
    private bool isSummonPlayer = true;
    private Color summonColor = Color.green; // 소환할때의 빛 색
    private Color takeColor = Color.red; // 무인도로 데려갈때의 빛 색

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CallCloud().Forget();
        }
    }

    public async UniTaskVoid CallCloud()
    {
        Vector3 actualPosition = playerTransform.position + Vector3.up * gapDistance;
        actualPosition.x = actualPosition.x - pivotModifier;
        await flyToSummonSpot(actualPosition, flyTime); // 플레이어 머리위로 날아오고
        await cloudResize(true); // 구름의 사이즈를 조절한다
        emitSpotLight(isSummonPlayer);
    }


    private void emitSpotLight(bool isSummoningPlayer)
    {
        if (isSummoningPlayer)
            spotLight.color = summonColor;
        else
            spotLight.color = takeColor;


        spotLight.enabled = true;
    }


    private async UniTask cloudResize(bool shouldGetBigger)
    {
        if (shouldGetBigger)
        {
            await controlSize(initialSize, bigSize, sizeTime);
        }
        else
        {
            await controlSize(bigSize, disappearSize, sizeTime);
        }
    }

    private async UniTask flyToSummonSpot(Vector3 summonPosition, float flyTime)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 midPoint = Vector3.Lerp(initialPosition, summonPosition, 0.5f);
        float magnitude = Vector3.Magnitude(midPoint - summonPosition);
        Vector3 controlPoint = midPoint - Vector3.up * magnitude; // 크기를 곱해줘서 더 아래값을 가져올 수 있음

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
            cloudBody.localScale = Vector3.Lerp(startSize, endSize, elapsedTime / duration);
            cloudBody.localPosition = applyLocalPosition(startSize, endSize);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }


    private Vector3 applyLocalPosition(Vector3 startSize, Vector3 endSize)
    {

        Vector3 localPositionVector = Vector3.zero;
        float xFactor = 0.725f; // 인스펙터창에서 사이즈를 늘렸을때 localPosition이 아래와 같은 계수를 가지고 증가하는 규칙을 확인하였음
        float yFactor = 0.01f;
        float zFactor = 0.25f;

        if ((endSize - startSize).magnitude > 0f) // 구름 사이즈가 커져야한다는 의미임
        {
            localPositionVector.Set((1 - cloudBody.localScale.x) * xFactor, (1 - cloudBody.localScale.y) * yFactor, (1 - cloudBody.localScale.z) * zFactor);
        }
        else // 구름 사이즈가 작아져야한다는 의미임
        {
            localPositionVector.Set((cloudBody.localScale.x - 1) * xFactor, (cloudBody.localScale.y - 1) * yFactor, (cloudBody.localScale.z - 1) * zFactor);
        }

        return localPositionVector;
    }

}
