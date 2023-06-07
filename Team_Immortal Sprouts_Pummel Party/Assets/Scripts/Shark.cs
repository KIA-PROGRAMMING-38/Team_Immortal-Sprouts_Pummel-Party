using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Shark : MonoBehaviour
{
    [Header("---------------------Shark Rotate Around Island----------------------")]
    [SerializeField] private Transform sharkIslandTransform;
    [SerializeField] [Range(30f, 150f)] private float rotateSpeed = 40f;

    [Header("----------------------Shark Jump----------------------")]
    [SerializeField] private Transform bezierPoint;
    [SerializeField] private bool isAttack = false;
    [SerializeField] private float jumpTime = 2f;

    [Header("----------------------Shark Look At Player----------------------")]
    [SerializeField] private Transform playerPos;

    
    
    

    private void Start()
    {
        RotateAroundIsland().Forget();
        WaitUntilJump().Forget();
    }

    private Vector3 initialPosition;
    private float elapsedTime = 0f;
    private async UniTaskVoid WaitUntilJump()
    {
        await UniTask.WaitUntil(() => isAttack == true);

        oppositePosition = GetOppositePosition(transform.position, sharkIslandTransform.position);
        initialPosition = transform.position;
        while (elapsedTime <= jumpTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = SharkJumpTowardsOpposite(initialPosition, bezierPoint.position, oppositePosition, elapsedTime / jumpTime);
            await UniTask.Yield();
        }

        elapsedTime = 0f;
        isAttack = false;

        WaitUntilJump().Forget(); // 또 호출하면, 또 isAttack 이 true가 될떄까지 기다린다
    }


    private Vector3 rotateAxis = Vector3.up;
    private async UniTaskVoid RotateAroundIsland()
    {
        while (true)
        {
            while (!isAttack)
            {
                transform.RotateAround(sharkIslandTransform.position, rotateAxis, -rotateSpeed * Time.deltaTime);
                await UniTask.Yield();
            }
            await UniTask.Yield();
        }
    }


    private Vector3 oppositePosition;
    private Vector3 GetOppositePosition(Vector3 currentPosition, Vector3 sharkIslandPosition)
    {
        Vector3 offSet = sharkIslandPosition - currentPosition;
        offSet.y = currentPosition.y; // 상어는 타일보다 아래에 있으므로, y값을 다시 변경해준다

        return offSet + sharkIslandPosition;
    }

    private Vector3 SharkJumpTowardsOpposite(Vector3 initialPosition, Vector3 middleUpPosition, Vector3 targetPosition, float t)
    {
        Vector3 firstMiddle = PrimaryBezierCurve(initialPosition, middleUpPosition, t);
        Vector3 secondMiddle = PrimaryBezierCurve(middleUpPosition, targetPosition, t);

        return PrimaryBezierCurve(firstMiddle, secondMiddle, t);
    }
    private Vector3 PrimaryBezierCurve(Vector3 start, Vector3 end, float t) => Vector3.Lerp(start, end, t);
}
