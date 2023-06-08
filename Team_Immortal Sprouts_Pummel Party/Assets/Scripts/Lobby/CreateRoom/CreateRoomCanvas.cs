using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomCanvas : MonoBehaviour
{
    private LobbyCanvases _lobbyCanvases;

    /// <summary>
    /// Lobby를 구성하는 Canvas들이 서로 참조할 수 있도록 초기 세팅
    /// </summary>
    public void CanvasInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }

    /// <summary>
    /// Create Room Canvas를 활성화
    /// </summary>
    public void Active()
    {
        gameObject.SetActive(true);
    }
}
