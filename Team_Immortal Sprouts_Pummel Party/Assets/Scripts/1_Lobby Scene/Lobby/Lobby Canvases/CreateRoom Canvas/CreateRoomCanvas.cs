using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomCanvas : MonoBehaviourPunCallbacks
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

    /// <summary>
    /// Create Room Canvas를 비활성화
    /// </summary>
    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    [SerializeField] private TMP_Text _roomName;

    /// <summary>
    /// Create Room Canvas의 OK 버튼 입력 이벤트
    /// </summary>
    public void OnClick_OK()
    {
        if(!PhotonNetwork.IsConnected)
        {
            return;
        }

        RoomOptions option = new RoomOptions();
        option.BroadcastPropsChangeToAll = true;
        option.PublishUserId = true;
        option.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(_roomName.text, option, TypedLobby.Default);
    }

    /// <summary>
    /// Create Room Canvas의 Cancel 버튼 입력 이벤트
    /// </summary>
    public void OnClick_Cancel()
    {
        _lobbyCanvases.MultiGameCanvas.TurnOnRaycast();
        Deactive();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료");
        _lobbyCanvases.WaitingRoomCanvas.gameObject.SetActive(true);
        _lobbyCanvases.DeactiveLobbyCanvases();
    }

    private const short EXISTS_ROOM_NAME = 32766;
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"방 생성 실패 {message}");
        if(returnCode == EXISTS_ROOM_NAME)
        {
            ActiveFailedPanel();
        }
    }

    [SerializeField] private CreateRoomFailedPanel _createRoomFailedPanel;
    private void ActiveFailedPanel()
    {
        _createRoomFailedPanel.Active();
    }
}
