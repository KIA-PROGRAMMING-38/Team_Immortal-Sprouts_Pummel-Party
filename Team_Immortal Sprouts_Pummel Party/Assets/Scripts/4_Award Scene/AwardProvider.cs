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
    [SerializeField] private ParticleSystem popParticleSystem;

    [HideInInspector]
    public UnityEvent<Transform> OnGiveAward = new UnityEvent<Transform>();
    [HideInInspector]
    public UnityEvent OnAwardGiven = new UnityEvent();

    [Header("--------------- Golden Egg Control -----------------")]
    [SerializeField] private float eggAppearTime = 3f;
    [SerializeField] private float eggStayTime = 2f;
    [SerializeField] private float eggDisappearTime = 0.3f;
    
    private float spiralSpeed = 1000f;
    private float spiralRadius = 4f;
    private Vector3 initialEggSize;

    private void Awake()
    {
        initialEggSize = goldenEgg.transform.localScale;
        initWinnerList();
    }

    private void Start()
    {
        activateAwardLoop().Forget();
    }


    private List<int>[] playerLists;
    private const int MVP_INDEX = 0;
    private const int LOSER_INDEX = 1;
    private const int FIGHTER_INDEX = 2;
    private const int FINAL_WINNER_INDEX = 3;

    private void initWinnerList()
    {
        playerLists = new List<int>[4];

        playerLists[MVP_INDEX] = Statistics.GetMVPIndex();
        playerLists[LOSER_INDEX] = Statistics.GetLoserIndex();
        playerLists[FIGHTER_INDEX] = Statistics.GetFighterIndex();
        playerLists[FINAL_WINNER_INDEX] = Statistics.GetFinalWinner();
    }

    private async UniTaskVoid activateAwardLoop()
    {
        await UniTask.Delay(15000);

        for (int awardOrder = 0; awardOrder < SUB_AWARD_COUNT + 1; ++awardOrder) // 마지막 상이 존재하기에 +1 해줌
        {


            int playerNum = UnityEngine.Random.Range(1, 5);

            List<int> winnerList = Statistics.GetLoserIndex();

            foreach (int winnerCount in winnerList)
            {
                await giveAwardToPlayer(winnerCount);
            }

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

        if (SUB_AWARD_COUNT <= awardCount) // 마지막 상 수여
        {
            await goldenEggAppear(winnerTransform, initialEggSize * 2f);

            await UniTask.Delay(TimeSpan.FromSeconds(eggStayTime));

            await goldenEggDisappear(initialEggSize * 2f);
        }
        else
        {
            await goldenEggAppear(winnerTransform, initialEggSize);

            await UniTask.Delay(TimeSpan.FromSeconds(eggStayTime));

            await goldenEggDisappear(initialEggSize);

            ++awardCount;
            await UniTask.Delay(3000); // 테스트 => 플레이어의 승리 연출이 끝나면 으로 조건이 나중에 바껴야함
            OnAwardGiven?.Invoke(); // spotLight이 다시 랜덤하게 움직인다
        }
    }

    
    private async UniTask goldenEggAppear(Transform winnerTransform, Vector3 startSize)
    {
        goldenEgg.SetActive(true);
        goldenEgg.transform.localScale = startSize;

        Vector3 initialPosition = winnerTransform.position + Vector3.up * 5f;
        Vector3 targetPosition = winnerTransform.position + Vector3.up * 1.8f;

        await ExtensionMethod.Vector3LerpExtension(goldenEggWhole, initialPosition, targetPosition, eggAppearTime);
    }

    private async UniTask goldenEggDisappear(Vector3 startSize)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= eggDisappearTime)
        {
            goldenEgg.transform.localScale = Vector3.Lerp(startSize, Vector3.zero, elapsedTime / eggDisappearTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        popParticleSystem.Play();
        goldenEgg.SetActive(false);
    }



    #region 교수님한테 빠꾸먹은 노맛 sprial move

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

    #endregion
}
