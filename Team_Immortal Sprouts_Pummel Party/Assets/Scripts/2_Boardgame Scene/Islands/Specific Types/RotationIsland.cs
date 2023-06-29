using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationIsland : Island
{
    [SerializeField] private ArrowSwitch arrowSwitch;
    [SerializeField] [Range(1f, 2f)] private float rotateTime = 1.5f;
    private CinemachineBasicMultiChannelPerlin virtualCamProperty;

    [Header("---------------------------------------Camera Shake---------------------------------------")]
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] [Range(1f, 10f)] private float shakeIntensity = 5f;
    private Transform playerTransform; // 플레이어를 함께 회전시켜주기 위한 변수
    private Quaternion defaultRotation;
    private bool isRotationFinished;

    private void Start()
    {
        InitPositionSettings().Forget();
        SaveInitialRotation();
        virtualCamProperty = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    /// <summary>
    /// 방향 화살표를 띄우는 함수
    /// </summary>
    public void PopUpDirectionArrow(Transform playerTransform)
    {
        SetPlayerTransform(playerTransform); // 애시당초 화살표를 띄운다는것은 플레이어가 회전섬에 도착했다는 것이다
        arrowSwitch.TurnOnSwitch();
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
        ActivateResetRotation().Forget();
    }

    /// <summary>
    /// 섬을 다시 원상복구 회전시키는 함수
    /// </summary>
    public async UniTaskVoid ActivateResetRotation()
    {
        await UniTask.WaitUntil(() => GetPlayerPresence() == false); // 섬위에 플레이어가 없을떄까지 기다린다
        await UniTask.Delay(TimeSpan.FromSeconds(4f)); // 4초 기다리고

        Rotate(defaultRotation).Forget(); // 리셋
    }

    /// <summary>
    /// 회전의 종료여부를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetRotationStatus() => isRotationFinished;

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

        controlCameraShake(shakeIntensity); // 카메라 지진 활성화

        ExtensionMethod.QuaternionLerpExtension(transform, initialRotation, targetRotation, rotateTime).Forget(); // 섬 회전
        
        if (playerTransform != null) // 플레이어 회전
        {
            Quaternion playerInitialRotation = playerTransform.rotation;
            Quaternion playerTargetRotation = Quaternion.Euler(0f, playerInitialRotation.eulerAngles.y + targetRotation.eulerAngles.y, 0f);
            ExtensionMethod.QuaternionLerpExtension(playerTransform, playerInitialRotation, playerTargetRotation, rotateTime).Forget();
        }

        controlCameraShake(0f); // 지진 X
        

        if (isResetting == false) // 화살표 방향을 누르고, 회전이 끝나면 isRotationFinished를 true로 만들어준다
        {
            isRotationFinished = true;
            playerTransform.GetComponent<BoardPlayerController>().ControlCanMove(true);
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

    private void controlCameraShake(float intensity)
    {
        virtualCamProperty.m_AmplitudeGain = intensity;
    }

    public override void ActivateIsland(Transform playerTransform = null)
    {
        Debug.Log("방향 화살표 팝업 시킬꺼임");
        PopUpDirectionArrow(playerTransform);
    }
}
