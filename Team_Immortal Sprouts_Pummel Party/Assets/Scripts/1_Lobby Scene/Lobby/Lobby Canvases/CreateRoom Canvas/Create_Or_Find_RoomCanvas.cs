using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Create_Or_Find_RoomCanvas : MonoBehaviourPunCallbacks
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

    #region OnClick �̺�Ʈ �Լ�

    [SerializeField] private TMP_Text _roomName;
    /// <summary>
    /// Create Room Canvas�� OK ��ư �Է� �̺�Ʈ
    /// </summary>
    public void OnClick_OK()
    {
        if (_lobbyCanvases.MultiGameCanvas.isCreatingRoom == true) // �� ����� ��ư�� �����ٸ�
        {
            if (!PhotonNetwork.IsConnected)
            {
                return;
            }

            RoomOptions option = new RoomOptions();
            option.BroadcastPropsChangeToAll = true;
            option.PublishUserId = true;
            option.MaxPlayers = 4;

            PhotonNetwork.CreateRoom(_roomName.text, option, TypedLobby.Default); // ���� �����
            Debug.Log($"{_roomName.text}���� ��������ϴ�");
        }
        else // �� ã�� ��ư�� �����ٸ�
        {
            PhotonNetwork.JoinRoom(_roomName.text); // ���� ã�´�
            Debug.Log($"{_roomName.text}�濡 ���Խ��ϴ�");
        }
        
    }

    /// <summary>
    /// Create Room Canvas�� Cancel ��ư �Է� �̺�Ʈ
    /// </summary>
    public void OnClick_Cancel()
    {
        _lobbyCanvases.MultiGameCanvas.TurnOnRaycast();
        Deactive();
    }

    #endregion

    #region Photon Callback �̺�Ʈ �Լ�
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

    private const short NOT_EXIST_ROOM_CODE = 32758;
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"�� ���忡 �����Ͽ����ϴ�. {message}");
        if (returnCode == NOT_EXIST_ROOM_CODE)
        {
            ActiveFailedPanel();
        }
    }

    #endregion

    [SerializeField] private CreateRoomFailedPanel _createRoomFailedPanel;
    [SerializeField] private FindRoomFailedPanel _findRoomFailedPanel;
    private void ActiveFailedPanel()
    {
        if (_lobbyCanvases.MultiGameCanvas.isCreatingRoom == true) // �� ����⸦ �����ٸ�
        {
            _createRoomFailedPanel.Active();
        }
        else // �� ã�⸦ �����ٸ�
        {
            _findRoomFailedPanel.Active();
        }
    }


}
