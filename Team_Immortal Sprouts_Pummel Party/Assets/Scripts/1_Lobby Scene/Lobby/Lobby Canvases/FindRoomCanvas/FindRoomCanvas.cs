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
    /// Lobby를 구성하는 Canvas들이 서로 참조할 수 있도록 초기 세팅
    /// </summary>
    public void CanvasInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }

    /// <summary>
    /// Find Room Canvas를 활성화
    /// </summary>
    public void Active()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Find Room Canvas를 비활성화
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
        //Debug.Log($"방 {PhotonNetwork.CurrentRoom.Name}에 입장했습니다. ");
        //_lobbyCanvases.WaitingRoomCanvas.gameObject.SetActive(true);
        //_lobbyCanvases.DeactiveLobbyCanvases();
    }


    //private const short NOT_EXIST_ROOM_CODE = 32758;
    //public override void OnJoinRoomFailed(short returnCode, string message)
    //{
    //    Debug.Log($"방 입장에 실패하였습니다. {message}");
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
