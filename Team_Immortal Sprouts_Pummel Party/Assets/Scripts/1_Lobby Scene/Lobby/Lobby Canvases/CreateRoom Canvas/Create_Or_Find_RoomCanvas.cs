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

    [SerializeField] private TMP_Text roomName;
    private const int defaultLength = 1; 
    /// <summary>
    /// Create Room Canvas�� OK ��ư �Է� �̺�Ʈ
    /// </summary>
    public void OnClick_From_Room_OK()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }

        string ActualRoomName;

        if (_lobbyCanvases.MultiGameCanvas.GetIsCreatingRoom() == true) // �� ����� ��ư�� �����ٸ�
        {
            RoomOptions option = new RoomOptions();
            option.BroadcastPropsChangeToAll = true;
            option.PublishUserId = true;
            option.MaxPlayers = 4;

            if (roomName.text.Length == defaultLength) // �ƹ��͵� �Է����� �ʾ�����, text.Length == 1�� ���´ٴ°� ������� ���ؼ� Ȯ���Ͽ���
            {
                // ������ �ѹ��� �ش�
                int randomNumber = Random.Range(0, 10000);
                ActualRoomName = randomNumber.ToString();
            }
            else // �ڵ带 �Է��ߴٸ�
            {
                ActualRoomName = roomName.text;
            }
            
            PhotonNetwork.CreateRoom(ActualRoomName, option, TypedLobby.Default); // ���� �����
            PhotonNetwork.LoadLevel("WaitingRoomScene");
            Debug.Log($"{ActualRoomName}���� ��������ϴ�");
        }
        else // �� ã�� ��ư�� �����ٸ�
        {
            ActualRoomName= roomName.text;
            PhotonNetwork.JoinRoom(ActualRoomName); // �Էµ� �ڵ��� ���� ����
            Debug.Log($"{ActualRoomName}�濡 ���Խ��ϴ�");
            //_lobbyCanvases.WaitingRoomCanvas.gameObject.SetActive(true);
            //_lobbyCanvases.DeactiveLobbyCanvases();
            PhotonNetwork.LoadLevel("WaitingRoomScene");
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
        //Debug.Log("�� ���� �Ϸ�");
        //_lobbyCanvases.WaitingRoomCanvas.gameObject.SetActive(true);
        //_lobbyCanvases.DeactiveLobbyCanvases();
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

    private const short EXISTS_ROOM_NAME = 32766;
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"�� ���� ���� {message}");
        if (returnCode == EXISTS_ROOM_NAME)
        {
            ActiveFailedPanel();
        }
    }


    #endregion

    [SerializeField] private CreateRoomFailedPanel _createRoomFailedPanel;
    [SerializeField] private FindRoomFailedPanel _findRoomFailedPanel;
    private void ActiveFailedPanel()
    {
        if (_lobbyCanvases.MultiGameCanvas.GetIsCreatingRoom() == true) // �� ����⸦ �����ٸ�
        {
            _createRoomFailedPanel.Active();
        }
        else // �� ã�⸦ �����ٸ�
        {
            _findRoomFailedPanel.Active();
        }
    }

}
