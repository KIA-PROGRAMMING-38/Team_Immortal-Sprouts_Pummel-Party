using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class JumpShark : MonoBehaviour
{
    [Header("----------------------Shark Jump----------------------")]
    [SerializeField] private BaseShark baseShark;
    [SerializeField] private Transform sharkIsland;
    [SerializeField] private Transform bezierPoint;
    [SerializeField] private float jumpTime = 2f;
    [SerializeField] private float downSpeed = 10f;

    

    private void OnEnable()
    {
        SharkJump().Forget();
    }

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private async UniTaskVoid SharkJump()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        initialRotation = transform.rotation;

        float elapsedTime = 0f;

        oppositePosition = GetOppositePosition(transform.position, sharkIsland.position);
        initialPosition = transform.position;


        float initialZAngle = transform.rotation.eulerAngles.z;
        float targetZAngle = -transform.rotation.eulerAngles.z - 40f;
        float changeZAngle = 0f;
        Vector3 sharkRotation = transform.rotation.eulerAngles;

        while (elapsedTime <= jumpTime)
        {
            elapsedTime += Time.deltaTime;

            transform.position = SharkJumpTowardsOpposite(initialPosition, bezierPoint.position, oppositePosition, elapsedTime / jumpTime);

            changeZAngle = Lerp(initialZAngle, targetZAngle, elapsedTime / jumpTime);
            sharkRotation.Set(sharkRotation.x, sharkRotation.y, changeZAngle);
            transform.rotation = Quaternion.Euler(sharkRotation);

            await UniTask.Yield();
        }

        SharkGoDown(jumpTime).Forget();
    }

    private async UniTaskVoid SharkGoDown(float disappearTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= disappearTime)
        {
            elapsedTime += Time.deltaTime;
            SharkMoveDown(downSpeed * 0.01f);
            await UniTask.Yield();
        }

        transform.position = initialPosition;
        transform.rotation = initialRotation;
        baseShark.LetBaseSharkKnowAttackFinished();
        releasePlayer();
    }

    private void SharkMoveDown(float downSpeed)
    {
        Vector3 newPosition = Vector3.zero;
        float randomValue = UnityEngine.Random.Range(-downSpeed, downSpeed);
        newPosition.Set(transform.position.x - randomValue, transform.position.y - downSpeed, transform.position.z - randomValue);
        transform.position = newPosition;
    }


    private Vector3 oppositePosition;
    private Vector3 GetOppositePosition(Vector3 currentPosition, Vector3 sharkIslandPosition)
    {
        Vector3 offSet = sharkIslandPosition - currentPosition;
        offSet *= 1.5f;
        offSet.y = currentPosition.y;
        return offSet + sharkIslandPosition;
    }

    private Vector3 SharkJumpTowardsOpposite(Vector3 initialPosition, Vector3 middleUpPosition, Vector3 targetPosition, float t)
    {
        Vector3 firstMiddle = PrimaryBezierCurve(initialPosition, middleUpPosition, t);
        Vector3 secondMiddle = PrimaryBezierCurve(middleUpPosition, targetPosition, t);

        return PrimaryBezierCurve(firstMiddle, secondMiddle, t);
    }
    private Vector3 PrimaryBezierCurve(Vector3 start, Vector3 end, float t) => Vector3.Lerp(start, end, t);

    private float Lerp(float start, float end, float t)
    {
        return start + (end - start) * t;
    }

    private Transform playerTransform;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.gameObject.transform;
            kidnapPlayer(playerTransform);
        }
    }

    private void kidnapPlayer(Transform playerTransform) // 플레이어를 납치하는 함수
    {
        playerTransform.SetParent(transform);
    }

    private void releasePlayer() // 플레이어를 풀어주는 함수 
    {
        playerTransform.SetParent(null);
        playerTransform = null;
    }
}

