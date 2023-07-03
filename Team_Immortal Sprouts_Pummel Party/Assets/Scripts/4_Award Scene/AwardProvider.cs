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

    [HideInInspector]
    public UnityEvent<Transform> OnGiveAward = new UnityEvent<Transform>();
    [HideInInspector]
    public UnityEvent OnAwardGiven = new UnityEvent();

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

        for (int i = 0; i <= SUB_AWARD_COUNT ;++i)
        {
            int playerNum = UnityEngine.Random.Range(0,4);
            await giveAwardToPlayer(playerNum);
            await UniTask.Delay(5000);
        }
    }


    private const int SUB_AWARD_COUNT = 3;
    private int awardCount;
    private async UniTask giveAwardToPlayer(int playerEnterOrder)
    {
        Transform winnerTransform = playerTransforms[playerEnterOrder];
        OnGiveAward?.Invoke(winnerTransform); // 수상자를 spotLight으로 비춰준다

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

}
