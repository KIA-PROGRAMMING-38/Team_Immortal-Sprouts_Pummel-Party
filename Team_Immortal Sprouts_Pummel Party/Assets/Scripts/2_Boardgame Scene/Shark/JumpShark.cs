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
    [SerializeField] private Material[] jumpSharkMaterials;
    [SerializeField] private BaseShark baseShark;
    [SerializeField] private Transform sharkIsland;
    [SerializeField] private Transform bezierPoint;
    [SerializeField] private float jumpTime = 2f;
    [SerializeField] private float downSpeed = 10f;

    


    private void OnEnable()
    {
        SharkWaitJump().Forget();
    }

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private async UniTaskVoid SharkWaitJump()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        initialRotation = transform.rotation;

        float elapsedTime = 0f;
        for (int i = 0; i < jumpSharkMaterials.Length; ++i)
        {
            jumpSharkMaterials[i].SetFloat("_AlphaValue", 1);
        }

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

        RenderSharkDownTransparent(jumpTime).Forget();
    }

    private async UniTaskVoid RenderSharkDownTransparent(float transparentTime)
    {
        float max = 1f;
        float min = 0f;

        float _disappearTime = transparentTime;
        float elapsedTime = 0f;

        while (elapsedTime <= _disappearTime)
        {
            elapsedTime += Time.deltaTime;
            for (int i = 0; i < jumpSharkMaterials.Length; ++i)
            {
                float alpha = Lerp(max, min, elapsedTime / _disappearTime);
                jumpSharkMaterials[i].SetFloat("_AlphaValue", alpha);
            }

            SharkMoveDown(downSpeed * 0.01f);
            await UniTask.Yield();
        }

        transform.position = initialPosition;
        transform.rotation = initialRotation;
        baseShark.LetBaseSharkKnowAttackFinished();
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
        offSet *= 2;
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

}

