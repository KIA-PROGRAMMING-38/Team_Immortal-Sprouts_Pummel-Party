using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomCanvas : MonoBehaviour
{
    private LobbyCanvases _lobbyCanvases;

    /// <summary>
    /// Lobby�� �����ϴ� Canvas���� ���� ������ �� �ֵ��� �ʱ� ����
    /// </summary>
    public void CanvasInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }

    /// <summary>
    /// Create Room Canvas�� Ȱ��ȭ
    /// </summary>
    public void Active()
    {
        gameObject.SetActive(true);
    }
}
