using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraTrace
{
    private static CinemachineVirtualCamera camera;
    private static CinemachineTransposer bodyController;
    private static CinemachineComposer aimController;
    private static Vector3 defualtBodyOffset;
    private static Vector3 defualtAimOffset;
    
    /// <summary>
    /// 버츄얼카메라의 바디와 에임을 초기화시켜주는 함수
    /// </summary>
    /// <param name="virtualCam"></param>
    public static void InitVirtualCamSettings(CinemachineVirtualCamera virtualCam) // 보드게임매니저가 start 때 해줘야할듯?
    {
        camera = virtualCam;    
        bodyController = virtualCam.GetCinemachineComponent<CinemachineTransposer>();
        aimController = virtualCam.GetCinemachineComponent<CinemachineComposer>();
        defualtBodyOffset = bodyController.m_FollowOffset;
        defualtAimOffset = aimController.m_TrackedObjectOffset;
    }

    /// <summary>
    /// 추적이 해제된 상태에서 카메라가 다시 플레이어를 따라가게 함
    /// </summary>
    /// <param name="target"></param>
    /// <param name="camera"></param>
    public static void ConnectFollow(Transform target, CinemachineVirtualCamera camera)
    {
        camera.Follow = target;
    }

    /// <summary>
    /// 카메라의 body Offset을 동적으로 변경하는 함수
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="offSet"></param>
    public static void ControlFollowOffset(CinemachineVirtualCamera camera, Vector3 offSet)
    {
        bodyController = camera.GetCinemachineComponent<CinemachineTransposer>();
        // 지금은 아직 뭐가 없어서 이런식으로 할때마다 불러와야할듯? 나중에는 이 getCinemachineComponent 없애줘야함
        bodyController.m_FollowOffset = offSet;
    }

    /// <summary>
    /// 바라봄이 해제된 상태에서 카메라가 다시 플레이어를 바라보게함
    /// </summary>
    /// <param name="target"></param>
    /// <param name="camera"></param>
    public static void ConnectLookAt(Transform target, CinemachineVirtualCamera camera)
    {
        camera.LookAt = target;
    }
    
    /// <summary>
    /// 카메라의 Aim Offset을 동적으로 변경하는 함수
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="offSet"></param>
    public static void ControlAimOffset(CinemachineVirtualCamera camera, Vector3 offSet)
    {
        aimController.m_TrackedObjectOffset = offSet;
    }
    

    /// <summary>
    /// 카메라의 추적을 중지
    /// </summary>
    /// <param name="camera"></param>
    public static void DisConnectFollow(CinemachineVirtualCamera camera)
    {
        camera.Follow = null;
        bodyController.m_FollowOffset = defualtBodyOffset;
    }

    /// <summary>
    /// 카메라의 바라봄을 중지
    /// </summary>
    /// <param name="camera"></param>
    public static void DisConnectLookAt(CinemachineVirtualCamera camera)
    {
        camera.LookAt = null;
        aimController.m_TrackedObjectOffset = defualtAimOffset;
    }
}
