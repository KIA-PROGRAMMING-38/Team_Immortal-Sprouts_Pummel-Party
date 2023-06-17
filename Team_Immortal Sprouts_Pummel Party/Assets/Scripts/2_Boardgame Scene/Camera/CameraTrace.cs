using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraTrace
{
    /// <summary>
    /// 추적이 해제된 상태에서 카메라가 다시 플레이어를 바라보게 함
    /// </summary>
    /// <param name="target"></param>
    /// <param name="camera"></param>
    public static void Connect(Transform target, CinemachineVirtualCamera camera)
    {
        camera.Follow = target;
    }

    /// <summary>
    /// 카메라의 추적을 중지
    /// </summary>
    /// <param name="camera"></param>
    public static void DisConnect(CinemachineVirtualCamera camera)
    {
        camera.Follow = null;
    }
}
