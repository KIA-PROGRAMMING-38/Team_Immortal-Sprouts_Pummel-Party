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
            giveAwardToPlayer(testNumber);
        }
    }


    private void giveFinalWinnerAward(int playerEnterOrder)
    {

    }


    private int miniAwardCount = 3;
    private void giveAwardToPlayer(int playerEnterOrder)
    {
        Transform winnerTransform = playerTransforms[playerEnterOrder];
        OnGiveAward?.Invoke(winnerTransform);
        playLightBombParticles(winnerTransform).Forget();
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
