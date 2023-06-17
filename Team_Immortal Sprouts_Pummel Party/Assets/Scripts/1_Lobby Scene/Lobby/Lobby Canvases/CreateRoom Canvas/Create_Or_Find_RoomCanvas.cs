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

            bool isCreatable = _lobbyCanvases.CheckIfRoomExist(ActualRoomName);

            if (!isCreatable) // �����ϴ� ���� ���ٸ�
            {
                PhotonNetwork.CreateRoom(ActualRoomName, option, TypedLobby.Default); // ���� �����
                PhotonNetwork.LoadLevel("WaitingRoom Scene");
            }
            else
            {
                ActiveFailedPanel();
            }
        }
        else // �� ã�� ��ư�� �����ٸ�
        {
            ActualRoomName = roomName.text;
            bool isJoinSuccess = _lobbyCanvases.CheckIfRoomExist(ActualRoomName);

            if (isJoinSuccess)
            {
                PhotonNetwork.JoinRoom(ActualRoomName); // �Էµ� �ڵ��� ���� ����
                Debug.Log($"{ActualRoomName}�濡 ���Խ��ϴ�");
                PhotonNetwork.LoadLevel("WaitingRoomScene");
            }
            else
            {
                Debug.Log($"�� ���忡 �����Ͽ����ϴ�.");
                ActiveFailedPanel();
            }
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
