using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AwardProvider : MonoBehaviour
{
    [SerializeField] private Transform[] playerTransforms;
    [SerializeField] private AwardLightController[] lightControllers;
    [SerializeField] private Transform goldenEggWhole;
    [SerializeField] private GameObject goldenEgg;

    [HideInInspector]
    public UnityEvent<Transform> OnGiveAward = new UnityEvent<Transform>();
    [HideInInspector]
    public UnityEvent OnAwardGiven = new UnityEvent();

    [Header("--------------- Golden Egg Control -----------------")]
    [SerializeField] private float eggAppearTime = 3f;
    [SerializeField] private float spiralSpeed = 1000f;
    [SerializeField] private float spiralRadius = 4f;

    private void Awake()
    {
    }

    private void Start()
    {
        testAwardLoop().Forget();
    }
    private async UniTaskVoid testAwardLoop()
    {
        await UniTask.Delay(15000);

        for (int i = 0; i <= SUB_AWARD_COUNT; ++i)
        {
            int playerNum = UnityEngine.Random.Range(1, 5);
            await giveAwardToPlayer(playerNum);
            await UniTask.Delay(5000);
        }
    }


    private const int SUB_AWARD_COUNT = 3;
    private int awardCount;
    private async UniTask giveAwardToPlayer(int playerEnterOrder)
    {
        Transform winnerTransform = playerTransforms[playerEnterOrder - 1];
        OnGiveAward?.Invoke(winnerTransform); // 수상자를 spotLight으로 비춰준다
        await goldenEggAppear(winnerTransform);

        if (SUB_AWARD_COUNT <= awardCount) // 마지막 상 수여
        {
            
        }
        else
        {
            ++awardCount;
            await UniTask.Delay(3000); // 테스트 => 플레이어의 승리 연출이 끝나면 으로 조건이 나중에 바껴야함
            OnAwardGiven?.Invoke(); // spotLight이 다시 랜덤하게 움직인다
        }
    }

    
    private async UniTask goldenEggAppear(Transform winnerTransform)
    {
        goldenEgg.SetActive(true);

        Vector3 initialPosition = winnerTransform.position + Vector3.up * 5f;
        Vector3 targetPosition = winnerTransform.position + Vector3.up * 1.5f;

        eggSprialMovement(goldenEgg.transform, eggAppearTime).Forget();
        await ExtensionMethod.Vector3LerpExtension(goldenEggWhole, initialPosition, targetPosition, eggAppearTime);

        await UniTask.Delay(2000);

        eggSprialMovement(goldenEgg.transform, eggAppearTime).Forget();
        await ExtensionMethod.Vector3LerpExtension(goldenEggWhole, targetPosition, initialPosition, eggAppearTime);

        goldenEgg.SetActive(false);
    }

    private async UniTaskVoid eggSprialMovement(Transform eggTransform, float duration)
    {
        await spiralMove(eggTransform, duration * 0.5f, spiralRadius); // 절반은 점점 커지게
        await spiralMove(eggTransform, duration * 0.5f, -spiralRadius); // 절반은 점점 작아지게
    }

    private const float DEFAULT_MODIFIER = 0.1f;
    private async UniTask spiralMove(Transform eggTransform, float duration, float radiusIncrease)
    {
        float elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            eggTransform.Translate(radiusIncrease * Vector3.right * Time.deltaTime * DEFAULT_MODIFIER);
            eggTransform.RotateAround(goldenEggWhole.position, Vector3.up, spiralSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }
}
