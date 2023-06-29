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
        sharkWaitJump().Forget();
    }

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private async UniTaskVoid sharkWaitJump()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        initialRotation = transform.rotation;

        float elapsedTime = 0f;

        oppositePosition = getOppositePosition(transform.position, sharkIsland.position);
        initialPosition = transform.position;


        float initialZAngle = transform.rotation.eulerAngles.z;
        float targetZAngle = -transform.rotation.eulerAngles.z - 40f;
        float changeZAngle = 0f;
        Vector3 sharkRotation = transform.rotation.eulerAngles;

        while (elapsedTime <= jumpTime)
        {
            elapsedTime += Time.deltaTime;

            transform.position = sharkJumpTowardsOpposite(initialPosition, bezierPoint.position, oppositePosition, elapsedTime / jumpTime);

            changeZAngle = lerp(initialZAngle, targetZAngle, elapsedTime / jumpTime);
            sharkRotation.Set(sharkRotation.x, sharkRotation.y, changeZAngle);
            transform.rotation = Quaternion.Euler(sharkRotation);

            await UniTask.Yield();
        }

        sharkGoDownStraight(jumpTime).Forget();
    }

    private async UniTaskVoid sharkGoDownStraight(float disappearTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= disappearTime)
        {
            elapsedTime += Time.deltaTime;
            sharkMoveDown(downSpeed * 0.01f);
            await UniTask.Yield();
        }

        transform.position = initialPosition;
        transform.rotation = initialRotation;
        baseShark.LetBaseSharkKnowAttackFinished();
        releasePlayer();
    }

    private void sharkMoveDown(float downSpeed)
    {
        Vector3 newPosition = Vector3.zero;
        float randomValue = UnityEngine.Random.Range(-downSpeed, downSpeed);
        newPosition.Set(transform.position.x - randomValue, transform.position.y - downSpeed, transform.position.z - randomValue);
        transform.position = newPosition;
    }


    private Vector3 oppositePosition;
    private Vector3 getOppositePosition(Vector3 currentPosition, Vector3 sharkIslandPosition)
    {
        Vector3 offSet = sharkIslandPosition - currentPosition;
        offSet *= 1.5f; // 조금 더 멀리 보내고싶음
        offSet.y = currentPosition.y;
        return offSet + sharkIslandPosition;
    }

    private Vector3 sharkJumpTowardsOpposite(Vector3 initialPosition, Vector3 middleUpPosition, Vector3 targetPosition, float t)
    {
        Vector3 firstMiddle = primaryBezierCurve(initialPosition, middleUpPosition, t);
        Vector3 secondMiddle = primaryBezierCurve(middleUpPosition, targetPosition, t);

        return primaryBezierCurve(firstMiddle, secondMiddle, t);
    }
    private Vector3 primaryBezierCurve(Vector3 start, Vector3 end, float t) => Vector3.Lerp(start, end, t);

    private float lerp(float start, float end, float t)
    {
        return start + (end - start) * t;
    }


    private Transform playerTransform;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.gameObject.GetComponent<Transform>();
            kidnapPlayer(playerTransform);
        }
    }

    private void kidnapPlayer(Transform playerTransform)
    {
        playerTransform.SetParent(transform);
        //playerTransform.GetComponent<BoardPlayerController>().ChangeToDraggedState();
        playerTransform.GetComponent<BoardPlayerController>().ChangeToDesiredState(BoardgamePlayerAnimID.DRAGGED);
    }

    private void releasePlayer()
    {
        playerTransform.SetParent(null);
        playerTransform = null;
    }
}

