using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedJoinRoomCanvas : MonoBehaviour
{
    private LobbyCanvases _lobbyCanvases;

    /// <summary>
    /// Lobby�� �����ϴ� Canvas���� ���� ������ �� �ֵ��� �ʱ� ����
    /// </summary>
    public void CanvasInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }
    public void OnClick_OkButton()
    {
        gameObject.SetActive(false);
    }
}
