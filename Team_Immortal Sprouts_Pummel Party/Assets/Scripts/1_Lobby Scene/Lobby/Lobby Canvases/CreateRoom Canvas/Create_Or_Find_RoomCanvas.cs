using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;

public class Create_Or_Find_RoomCanvas : MonoBehaviourPunCallbacks
{
    private LobbyCanvases _lobbyCanvases;

    //추가
    private LoadingScene levelLoader;
    private void Awake()
    {
        levelLoader = FindObjectOfType<LoadingScene>();
    }

   
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
    public void OnClick_From_Room_OK()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }

        string ActualRoomName;

        if (_lobbyCanvases.MultiGameCanvas.GetIsCreatingRoom() == true) // 방 만들기 버튼을 눌렀다면
        {
            RoomOptions option = new RoomOptions();
            option.BroadcastPropsChangeToAll = true;
            option.PublishUserId = true;
            option.MaxPlayers = 4;

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

            bool isCreatable = RootManager.DataManager.Room.CheckIfRoomExist(ActualRoomName);

            if (!isCreatable) // 존재하는 방이 없다면
            {
                PhotonNetwork.CreateRoom(ActualRoomName, option, TypedLobby.Default); // 방을 만든다
                //PhotonNetwork.LoadLevel(1);
                _lobbyCanvases.LoadBoardGame();
                levelLoader.BoardGameLoadPlay();
            }
            else
            {
                ActiveFailedPanel();
            }
        }
        else // 방 찾기 버튼을 눌렀다면
        {
            ActualRoomName = roomName.text;
            bool isJoinSuccess = RootManager.DataManager.Room.CheckIfRoomExist(ActualRoomName);

            if (isJoinSuccess)
            {
                PhotonNetwork.JoinRoom(ActualRoomName); // 입력된 코드의 방을 들어간다
                //PhotonNetwork.LoadLevel(1);
                _lobbyCanvases.LoadBoardGame();
                levelLoader.BoardGameLoadPlay(); 

            }
            else
            {
                Debug.Log($"방 입장에 실패하였습니다.");
                ActiveFailedPanel();
            }
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

    [SerializeField] private CreateRoomFailedPanel _createRoomFailedPanel;
    [SerializeField] private FindRoomFailedPanel _findRoomFailedPanel;
    private void ActiveFailedPanel()
    {
        if (_lobbyCanvases.MultiGameCanvas.GetIsCreatingRoom() == true) // 방 만들기를 눌렀다면
        {
            _createRoomFailedPanel.Active();
        }
        else // 방 찾기를 눌렀다면
        {
            _findRoomFailedPanel.Active();
        }
    }
}
