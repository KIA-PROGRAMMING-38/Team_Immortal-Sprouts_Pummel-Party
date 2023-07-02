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
    [SerializeField] private ParticleSystem[] lightBombParticles;

    [HideInInspector]
    public UnityEvent<Transform> OnGiveAward = new UnityEvent<Transform>();
    public UnityEvent OnAwardGiven = new UnityEvent();
    [Range(0, 3)] public int testNumber;

    private void Awake()
    {
        lightBombDelayTime = lightControllers[0].GetReachTime() + 0.5f;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            giveAwardToPlayer(testNumber).Forget();
        }
    }


    private const int SUB_AWARD_COUNT = 3;
    private int awardCount;
    private async UniTaskVoid giveAwardToPlayer(int playerEnterOrder)
    {
        Transform winnerTransform = playerTransforms[playerEnterOrder];
        OnGiveAward?.Invoke(winnerTransform); // 수상자를 spotLight으로 비춰준다

        if (SUB_AWARD_COUNT <= awardCount) // 마지막 상 수여
        {
            await playLightBombParticles(winnerTransform);
            await playLightBombParticles(winnerTransform);
            await playLightBombParticles(winnerTransform);
            await playLightBombParticles(winnerTransform);
        }
        else
        {
            ++awardCount;
            await playLightBombParticles(winnerTransform);
            await UniTask.Delay(3000); // 테스트 => 플레이어의 승리 연출이 끝나면 으로 조건이 나중에 바껴야함
            OnAwardGiven?.Invoke(); // spotLight이 다시 랜덤하게 움직인다
        }
    }

    private float lightBombDelayTime;
    [SerializeField] private float lightBombGapTime = 0.1f;
    private async UniTask playLightBombParticles(Transform winnerTransform)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(lightBombDelayTime)); // 조명이 비출때까지 기다린다

        for (int playCount = 0; playCount < lightBombParticles.Length; ++playCount)
        {
            Vector3 offSet = UnityEngine.Random.insideUnitCircle * 2f;
            lightBombParticles[playCount].gameObject.SetActive(true); // 지가 알아서 자동으로 꺼지기 떄문에 SetActive(False) 불필요
            lightBombParticles[playCount].transform.position = winnerTransform.position + offSet;
            lightBombParticles[playCount].Play();
            await UniTask.Delay(TimeSpan.FromSeconds(lightBombGapTime));
        }
    }
}
