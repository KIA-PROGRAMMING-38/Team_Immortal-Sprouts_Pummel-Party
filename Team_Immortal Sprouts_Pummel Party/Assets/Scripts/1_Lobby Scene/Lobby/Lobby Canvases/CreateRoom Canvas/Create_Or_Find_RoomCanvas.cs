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

    #region OnClick 이벤트 함수

    [SerializeField] private TMP_Text roomName;
    private const int defaultLength = 1; 
    /// <summary>
    /// Create Room Canvas의 OK 버튼 입력 이벤트
    /// </summary>
    public void OnClick_OK()
    {
        if (_lobbyCanvases.MultiGameCanvas.isCreatingRoom == true) // 방 만들기 버튼을 눌렀다면
        {
            if (!PhotonNetwork.IsConnected)
            {
                return;
            }

            RoomOptions option = new RoomOptions();
            option.BroadcastPropsChangeToAll = true;
            option.PublishUserId = true;
            option.MaxPlayers = 4;

            string ActualRoomName;

            if (roomName.text.Length == defaultLength) // 아무것도 입력하지 않았을때, text.Length == 1이 나온다는걸 디버깅을 통해서 확인하였음
            {
                // 랜덤한 넘버를 준다
                int randomNumber = Random.Range(0, 10000);
                ActualRoomName = randomNumber.ToString();
            }
            else // 코드를 입력했다면
            {
                ActualRoomName = roomName.text;
            }

            
            PhotonNetwork.CreateRoom(ActualRoomName, option, TypedLobby.Default); // 방을 만든다
            Debug.Log($"{ActualRoomName}방을 만들었습니다");
        }
        else // 방 찾기 버튼을 눌렀다면
        {
            PhotonNetwork.JoinRoom(roomName.text); // 입력된 코드의 방을 들어간다
            Debug.Log($"{roomName.text}방에 들어왔습니다");
            _lobbyCanvases.WaitingRoomCanvas.gameObject.SetActive(true);
            _lobbyCanvases.DeactiveLobbyCanvases();
        }
        
    }

    /// <summary>
    /// Create Room Canvas의 Cancel 버튼 입력 이벤트
    /// </summary>
    public void OnClick_Cancel()
    {
        _lobbyCanvases.MultiGameCanvas.TurnOnRaycast();
        Deactive();
    }

    #endregion

    #region Photon Callback 이벤트 함수
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

    private const short NOT_EXIST_ROOM_CODE = 32758;
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"방 입장에 실패하였습니다. {message}");
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
        if (_lobbyCanvases.MultiGameCanvas.isCreatingRoom == true) // 방 만들기를 눌렀다면
        {
            _createRoomFailedPanel.Active();
        }
        else // 방 찾기를 눌렀다면
        {
            _findRoomFailedPanel.Active();
        }
    }


}
