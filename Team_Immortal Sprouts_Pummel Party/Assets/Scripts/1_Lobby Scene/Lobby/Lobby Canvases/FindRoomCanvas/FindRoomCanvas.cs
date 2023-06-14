using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class FindRoomCanvas : MonoBehaviourPunCallbacks
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
    /// Find Room Canvas�� Ȱ��ȭ
    /// </summary>
    public void Active()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Find Room Canvas�� ��Ȱ��ȭ
    /// </summary>
    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    [SerializeField] private TMP_Text _roomName;
    public void OnClick_OK()
    {
        PhotonNetwork.JoinRoom(_roomName.text);
    }

    public void OnClick_Cancel()
    {
        _lobbyCanvases.MultiGameCanvas.TurnOnRaycast();
        Deactive();
    }

    public override void OnJoinedRoom()
    {
        //Debug.Log($"�� {PhotonNetwork.CurrentRoom.Name}�� �����߽��ϴ�. ");
        //_lobbyCanvases.WaitingRoomCanvas.gameObject.SetActive(true);
        //_lobbyCanvases.DeactiveLobbyCanvases();
    }


    //private const short NOT_EXIST_ROOM_CODE = 32758;
    //public override void OnJoinRoomFailed(short returnCode, string message)
    //{
    //    Debug.Log($"�� ���忡 �����Ͽ����ϴ�. {message}");
    //    if(returnCode == NOT_EXIST_ROOM_CODE)
    //    {
    //        ActiveFailedPanel();
    //    }
    //}

    [SerializeField] private FindRoomFailedPanel _findRoomFailedPanel;
    private void ActiveFailedPanel()
    {
        _findRoomFailedPanel.Active();
    }
}
