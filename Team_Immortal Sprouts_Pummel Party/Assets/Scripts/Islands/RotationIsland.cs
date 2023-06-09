using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationIsland : Island
{
    [SerializeField] [Range(1f, 2f)] private float rotateTime = 1.5f;
    private CinemachineBasicMultiChannelPerlin virtualCamProperty;

    [Header("---------------------------------------Camera Shake---------------------------------------")]
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] [Range(1f, 10f)] private float shakeIntensity = 5f;
    private Transform playerTransform; // 플레이어를 함께 회전시켜주기 위한 변수
    private Quaternion defaultRotation;
    private bool isRotationFinished;

    void Start()
    {
        InitPositionSettings().Forget();
        SaveInitialRotation();
        virtualCamProperty = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    /// <summary>
    /// 섬과 함께 회전시킬 플레이어를 설정하는 함수
    /// </summary>
    /// <param name="playerTransform"></param>
    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    /// <summary>
    /// 섬을 누른 화살표 방향으로 회전시키는 함수
    /// </summary>
    /// <param name="targetRotation"></param>
    public void ActivateRotatation(Quaternion targetRotation)
    {
        Rotate(targetRotation).Forget();
    }

    /// <summary>
    /// 섬을 다시 원상복구 회전시키는 함수
    /// </summary>
    public void ActivateResetRotation()
    {
        Rotate(defaultRotation).Forget();
    }

    /// <summary>
    /// 회전의 종료여부를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetRotationStatus()
    {
        return isRotationFinished;
    }

    private async UniTaskVoid Rotate(Quaternion targetRotation)
    {
        Quaternion initialRotation;
        bool isResetting = false;

        if (targetRotation == defaultRotation) // 다시 되돌릴 때
        {
            SetNextPosition(Vector3.zero); // 다음 목적지를 없애준다
            ResetPlayerTransform(); // 같이 회전시킬 플레이어를 없애준다
            initialRotation = transform.rotation;
            isResetting = true;
        }
        else // 화살표를 눌렀을 때
        {
            initialRotation = defaultRotation;
        }

        float elapsedTime = 0f;
        
        ActivateCameraShake();

        if (playerTransform == null) // 플레이어가 섬위에 없다면
        {
            while (elapsedTime <= rotateTime) // 섬만 회전시킨다
            {
                transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotateTime);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }
        else // 플레이어가 섬위에 존재한다면
        {
            while (elapsedTime <= rotateTime) // 플레이어와 섬을 같이 회전시킨다
            {
                transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotateTime);
                playerTransform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotateTime);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }

        DeactivateCameraShake();
        

        if (isResetting == false) // 화살표 방향을 누르고, 회전이 끝나면 isRotationFinished를 true로 만들어준다
        {
            isRotationFinished = true;
        }
        else // 다시 원상복구 시키는 회전이라면 애초에 isRotationFinished를 사용할 일이 없다
        {
            isRotationFinished = false;
        }
    }

    private void SaveInitialRotation()
    {
        defaultRotation = transform.rotation;
    }

    private void ResetPlayerTransform()
    {
        playerTransform = null;
    }

    private void ActivateCameraShake()
    {
        virtualCamProperty.m_AmplitudeGain = shakeIntensity;
    }

    private void DeactivateCameraShake()
    {
        virtualCamProperty.m_AmplitudeGain = 0f;
    }
}
