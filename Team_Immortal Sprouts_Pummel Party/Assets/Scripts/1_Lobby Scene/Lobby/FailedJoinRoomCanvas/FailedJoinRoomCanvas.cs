using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedJoinRoomCanvas : MonoBehaviour
{
    private LobbyCanvases _lobbyCanvases;

    /// <summary>
    /// Lobby를 구성하는 Canvas들이 서로 참조할 수 있도록 초기 세팅
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
