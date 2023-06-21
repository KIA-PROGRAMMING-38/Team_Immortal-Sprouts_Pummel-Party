using Cysharp.Threading.Tasks;
using Photon.Pun.Simple;
using System;
using System.Threading;
using UnityEngine;


public static class ExtensionMethod
{
    #region 이동 함수

    /// <summary>
    /// 이동 보간 함수
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="startVector"></param>
    /// <param name="targetVector"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static async UniTaskVoid Vector3LerpExtension(Transform transform, Vector3 startVector, Vector3 targetVector, float duration)
    {
        if (transform == null || transform.position == targetVector || startVector == targetVector)
        {
            return;
        }

        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            transform.position = Vector3.Lerp(startVector, targetVector, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// target을 smoothDamping 하여 쫓아가는 함수(smoothTime이 커질수록 느리게 따라감)
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="targetTransform"></param>
    /// <param name="duration"></param>
    /// <param name="smoothTime"></param>
    /// <returns></returns>
    public static async UniTaskVoid SmoothDampFollow(Transform transform, Transform targetTransform, float duration, float smoothTime)
    {
        if (transform == null || targetTransform == null || duration == 0)
        {
            return;
        }

        Vector3 refVector = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetTransform.position, ref refVector, smoothTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }



    /// <summary>
    /// UniTaskVoid 타입의 2차 베지어 곡선 이동 함수
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="startVector"></param>
    /// <param name="controlVector"></param>
    /// <param name="targetVector"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static async UniTaskVoid SecondaryBezierVoidCurve(Transform transform, Vector3 startVector, Vector3 controlVector, Vector3 targetVector, float duration)
    {
        if (transform == null || transform.position == targetVector || startVector == targetVector)
        {
            return;
        }

        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            float t = elapsedTime / duration;
            Vector3 m0 = Vector3.Lerp(startVector, targetVector, t);
            Vector3 m1 = Vector3.Lerp(controlVector, targetVector, t);
            transform.position = Vector3.Lerp(m0, m1, t);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// UniTask 타입의 2차 베지어 곡선 이동함수
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="startVector"></param>
    /// <param name="controlVector"></param>
    /// <param name="targetVector"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static async UniTask SecondaryBezierCurve(Transform transform, Vector3 startVector, Vector3 controlVector, Vector3 targetVector, float duration)
    {
        if (transform == null || transform.position == targetVector || startVector == targetVector)
        {
            return;
        }

        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            float t = elapsedTime / duration;
            Vector3 m0 = Vector3.Lerp(startVector, targetVector, t);
            Vector3 m1 = Vector3.Lerp(controlVector, targetVector, t);
            transform.position = Vector3.Lerp(m0, m1, t);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }


    /// <summary>
    /// 3차 베지어 곡선 이동 함수
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="startVector"></param>
    /// <param name="controlVector1"></param>
    /// <param name="controlVector2"></param>
    /// <param name="targetVector"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static async UniTaskVoid TertiaryBezierCurve(Transform transform, Vector3 startVector, Vector3 controlVector1, Vector3 controlVector2, Vector3 targetVector, float duration)
    {
        if (transform == null || transform.position == targetVector || startVector == targetVector)
        {
            return;
        }


        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            float t = elapsedTime / duration;
            Vector3 m0 = Vector3.Lerp(startVector, controlVector1, t);
            Vector3 m1 = Vector3.Lerp(controlVector1, controlVector2, t);
            Vector3 m2 = Vector3.Lerp(controlVector2, targetVector, t);

            Vector3 mm1 = Vector3.Lerp(m0, m1, t);
            Vector3 mm2 = Vector3.Lerp(m1, m2, t);

            transform.position = Vector3.Lerp(mm1, mm2, t);

            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    #endregion



    #region 회전 함수

    /// <summary>
    /// 회전 보간 함수
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="startRotation"></param>
    /// <param name="targetRotation"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static async UniTaskVoid QuaternionLerpExtension(Transform transform, Quaternion startRotation, Quaternion targetRotation, float duration)
    {
        if (transform == null || transform.rotation == targetRotation || startRotation == targetRotation)
        {
            return;
        }

        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// axis 축을 기반으로 duration 만큼 회전시키는 함수
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="rotationSpeed"></param>
    /// <param name="axis"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static async UniTaskVoid DoRotate(Transform transform, float rotationSpeed, Vector3 axis, float duration)
    {
        if (transform == null || rotationSpeed == 0f)
        {
            return;
        }

        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            transform.Rotate(axis, rotationSpeed);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }


    /// <summary>
    /// target을 향해 천천히 smoothDamp마냥 catchUpSpeed와 비례하여 빠르게 어느정도 텀을 주고 바라보는 함수
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="targetTransform"></param>
    /// <param name="duration"></param>
    /// <param name="catchUpSpeed"></param>
    /// <returns></returns>
    public static async UniTaskVoid QuaternionSmoothDampRotation(Transform transform, Transform targetTransform, float duration, float catchUpSpeed)
    {
        if (transform == null || targetTransform == null || duration == 0 || catchUpSpeed == 0)
        {
            return;
        }

        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetTransform.rotation, catchUpSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    #endregion


    #region 색깔 함수

    /// <summary>
    /// 마티리얼의 색깔을 duration 동안 gapTime마다 바꿔주는 함수
    /// </summary>
    /// <param name="material"></param>
    /// <param name="initialColor"></param>
    /// <param name="newColor"></param>
    /// <param name="duration"></param>
    /// <param name="gapTime"></param>
    /// <returns></returns>
    public static async UniTaskVoid ChangeColor(Material material, Color initialColor, Color newColor, float duration, float gapTime)
    {
        if (initialColor == newColor || material == null)
        {
            return;
        }

        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            material.color = newColor;
            await UniTask.Delay(TimeSpan.FromSeconds(gapTime));
            material.color = initialColor;
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        material.color = initialColor;

    }


    #endregion


    #region 기타함수

    /// <summary>
    /// 리지드바디를 direction 방향으로 smashForce의 세기만큼 날려버리는 함수
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="direction"></param>
    /// <param name="smashForce"></param>
    public static void SmashRigidbody(Rigidbody rigidbody, Vector3 direction, float smashForce)
    {
        if (rigidbody == null)
        {
            return;
        }

        rigidbody.AddForce(direction * smashForce, ForceMode.Impulse);
    }

    /// <summary>
    /// 리지드바디를 랜덤축으로 rotateForce 세기만큼 회전시키는 함수
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="rotateForce"></param>
    /// <returns></returns>
    public static void RigidbodyRandomRotation(Rigidbody rigidbody, float rotateForce)
    {
        if (rigidbody == null)
        {
            return;
        }

        rigidbody.angularVelocity = UnityEngine.Random.insideUnitSphere * rotateForce;
    }

    /// <summary>
    /// target까지의 방향을 반환하는 함수
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="targetTransform"></param>
    /// <returns></returns>
    public static Vector3 GetDirection(Transform transform, Transform targetTransform)
    {
        Vector3 targetDirection = Vector3.zero;

        if (transform == null || targetTransform == null || transform.position == targetTransform.position)
        {
            return targetDirection;
        }

        targetDirection = targetTransform.position - transform.position;
        targetDirection.Normalize();
        return targetDirection;
    }


    /// <summary>
    /// 크기 1 * shakeIntensity 인 구 안에서의 흔들림을 구현하는 함수
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="shakeDuration"></param>
    /// <param name="shakeIntensity"></param>
    /// <returns></returns>
    public static async UniTaskVoid ShakeSpherePosition(Transform transform, float shakeDuration, float shakeIntensity)
    {
        if (transform == null || shakeDuration == 0 || shakeIntensity == 0)
        {
            return;
        }

        Vector3 initialPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime <= shakeDuration)
        {
            transform.position = UnityEngine.Random.insideUnitSphere * shakeIntensity;
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        transform.position = initialPosition;
    }

    #endregion








}

