using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class BaseShark : MonoBehaviour
{
    [Header("Shark Materials")]
    [SerializeField] private JumpShark jumpShark;

    [Header("---------------------Shark Rotate Around Island----------------------")]
    [SerializeField] private Transform sharkIslandTransform;
    [SerializeField][Range(30f, 150f)] private float rotateSpeed = 40f;
    [SerializeField] private float disappearTime = 2f;

    private bool isAttackFinished = false;

    private CancellationTokenSource playResource;
    private CancellationTokenSource cancelResource;
    private CancellationToken token;
    private void Awake()
    {
        playResource = new CancellationTokenSource();
        cancelResource = new CancellationTokenSource();
        cancelResource.Cancel();
        token = playResource.Token;
    }

    private void Start()
    {
        rotateAroundIsland().Forget();
    }

    private void OnDestroy()
    {
        token = cancelResource.Token;
    }

    /// <summary>
    /// 상어의 공격을 활성화하는 함수입니다
    /// </summary>
    public void JumpAttack()
    {
        sharkGoDownWater(disappearTime).Forget();
    }

    private Vector3 rotateAxis = Vector3.up;
    private async UniTaskVoid rotateAroundIsland()
    {
        while (true)
        {
            transform.RotateAround(sharkIslandTransform.position, rotateAxis, -rotateSpeed * Time.deltaTime);
            await UniTask.Yield(token);
        }
    }

    private async UniTaskVoid sharkGoDownWater(float disappearTime)
    {
        token = cancelResource.Token; // 상어 회전 멈춤

        float elapsedTime = 0f;

        while (elapsedTime <= disappearTime)
        {
            elapsedTime += Time.deltaTime;
            sharkMoveDown(true);
            await UniTask.Yield();
        }
        jumpShark.gameObject.SetActive(true);
    }

    private async UniTaskVoid sharkComeUpWater(float disappearTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= disappearTime)
        {
            elapsedTime += Time.deltaTime;
            sharkMoveDown(false);
            await UniTask.Yield();
        }

        isAttackFinished = false;
        token = playResource.Token;
        rotateAroundIsland().Forget();
    }

    /// <summary>
    /// 상어의 공격이 끝남을 베이스 상어에게 알려주고, 점프상어를 꺼준다
    /// </summary>
    public void LetBaseSharkKnowAttackFinished()
    {
        isAttackFinished = true;
        jumpShark.gameObject.SetActive(false);
        sharkComeUpWater(disappearTime).Forget();
    }

    private void sharkMoveDown(bool isDown)
    {
        int down = -1;

        if (!isDown)
        {
            down *= -1;
        }

        transform.Translate(transform.up * down * Time.deltaTime);
    }

    private float lerp(float start, float end, float t)
    {
        return start + (end - start) * t;
    }
}
