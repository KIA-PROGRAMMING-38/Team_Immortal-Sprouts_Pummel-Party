using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraTrace
{
    /// <summary>
    /// ������ ������ ���¿��� ī�޶� �ٽ� �÷��̾ �ٶ󺸰� ��
    /// </summary>
    /// <param name="target"></param>
    /// <param name="camera"></param>
    public static void Connect(Transform target, CinemachineVirtualCamera camera)
    {
        camera.Follow = target;
    }

    /// <summary>
    /// ī�޶��� ������ ����
    /// </summary>
    /// <param name="camera"></param>
    public static void DisConnect(CinemachineVirtualCamera camera)
    {
        camera.Follow = null;
    }
}
