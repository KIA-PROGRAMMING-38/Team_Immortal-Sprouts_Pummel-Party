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

    /// <summary>
    /// Create Room Canvas�� ��Ȱ��ȭ
    /// </summary>
    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    [SerializeField] private TMP_Text _roomName;

    /// <summary>
    /// Create Room Canvas�� OK ��ư �Է� �̺�Ʈ
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
    /// Create Room Canvas�� Cancel ��ư �Է� �̺�Ʈ
    /// </summary>
    public void OnClick_Cancel()
    {
        _lobbyCanvases.MultiGameCanvas.TurnOnRaycast();
        Deactive();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("�� ���� �Ϸ�");
        _lobbyCanvases.WaitingRoomCanvas.gameObject.SetActive(true);
        _lobbyCanvases.DeactiveLobbyCanvases();
    }

    private const short EXISTS_ROOM_NAME = 32766;
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"�� ���� ���� {message}");
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
