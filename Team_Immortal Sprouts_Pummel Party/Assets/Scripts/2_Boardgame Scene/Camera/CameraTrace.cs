using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraTrace
{
    private static CinemachineVirtualCamera virtualCam;
    private static CinemachineTransposer bodyInfo;
    private static CinemachineComposer aimInfo;
    private static Vector3 defaultBodyOffset;
    private static Vector3 defaultAimOffset;

    /// <summary>
    /// 카메라 초기세팅을 위한 함수 => 나중에 프레임워크가 해줘야함
    /// </summary>
    /// <param name="virtualCamera"></param>
    /// <param name="playerTransform"></param>
    public static void InitializeCamera(CinemachineVirtualCamera virtualCamera, Transform playerTransform)
    {
        virtualCam = virtualCamera;
        virtualCam.Follow = playerTransform;
        virtualCam.LookAt = playerTransform;
        bodyInfo = virtualCam.GetCinemachineComponent<CinemachineTransposer>();
        defaultBodyOffset = bodyInfo.m_FollowOffset;
        aimInfo = virtualCam.GetCinemachineComponent<CinemachineComposer>();
        defaultAimOffset = aimInfo.m_TrackedObjectOffset;
    }

    /// <summary>
    /// 카메라의 Follow를 설정해주는 함수
    /// </summary>
    /// <param name="target"></param>
    /// <param name="camera"></param>
    public static void ConnectFollow(Transform target) => virtualCam.Follow = target;

    /// <summary>
    /// 카메라의 LookAt을 설정해주는 함수
    /// </summary>
    /// <param name="target"></param>
    public static void ConnectLookAt(Transform target) => virtualCam.LookAt = target;

    /// <summary>
    /// 카메라의 Follow를 해제하는 함수
    /// </summary>
    /// <param name="camera"></param>
    public static void DisConnectFollow() => virtualCam.Follow = null;

    /// <summary>
    /// 카메라의 LookAt을 해제하는 함수
    /// </summary>
    public static void DisconnectLookAt() => virtualCam.LookAt = null;

    /// <summary>
    /// 카메라의 Follow나 Aim 의 offset을 변경할 수 있는 함수
    /// </summary>
    /// <param name="isBody"></param>
    /// <param name="desiredOffset"></param>
    public static void ChangeOffset(bool isBody, Vector3 desiredOffset)
    {
        if (isBody)
        {
            bodyInfo.m_FollowOffset = desiredOffset;
        }
        else
        {
            aimInfo.m_TrackedObjectOffset = desiredOffset;
        }
    }

    /// <summary>
    /// 카메라의 offset을 리셋해주는 함수
    /// </summary>
    public static void ResetOffSet()
    {
        bodyInfo.m_FollowOffset = defaultBodyOffset;
        aimInfo.m_TrackedObjectOffset = defaultAimOffset;
    }
}
