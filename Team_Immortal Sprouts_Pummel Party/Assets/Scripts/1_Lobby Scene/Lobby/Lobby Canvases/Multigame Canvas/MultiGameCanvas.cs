using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiGameCanvas : MonoBehaviour
{
    private LobbyCanvases _lobbyCanvases;
    private CanvasGroup _canvasGroup;
    [SerializeField] private bool isCreatingRoom; // 테스트 위해 SerializeField 입력
    [SerializeField] GameObject failedJoinRoomCanvas;

    /// <summary>
    /// Lobby를 구성하는 Canvas들이 서로 참조할 수 있도록 초기 세팅
    /// </summary>
    public void CanvasInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }


    #region OnClick Events

    /// <summary>
    /// 게임스타트 버튼 클릭시 작동하는 이벤트 함수
    /// </summary>
    public void OnClick_GameStartButton()
    {
        OnJoinRandomRoom();
    }

    /// <summary>
    /// Create 또는 Find Room 버튼 클릭시 작동하는 이벤트 함수
    /// </summary>
    public void OnClick_CreateRoom()
    {
        _lobbyCanvases.Create_Or_Find_RoomCanvas.Active();
        isCreatingRoom = true;
        TurnOffRaycast();
    }

    /// <summary>
    /// Find Room 버튼 클릭시 작동하는 이벤트 함수
    /// </summary>
    public void OnClick_FindRoom()
    {
        _lobbyCanvases.Create_Or_Find_RoomCanvas.Active();
        isCreatingRoom = false;
        TurnOffRaycast();
    }


    /// <summary>
    /// 게임종료 버튼 클릭시 작동하는 이벤트 함수
    /// </summary>
    public void OnClick_LeaveGameButton()
    {
        Application.Quit();
    }

    #endregion

    /// <summary>
    /// MultiGame Canvas에 raycast 입력을 차단
    /// </summary>
    public void TurnOffRaycast()
    {
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// MultiGame Canvas에 raycast 입력을 받을 수 있도록 설정
    /// </summary>
    public void TurnOnRaycast()
    {
        _canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// MultiGame Canvas를 활성화
    /// </summary>
    public void Active()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// MultiGame Canvas를 비활성화
    /// </summary>
    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    public bool GetIsCreatingRoom()
    {
        return isCreatingRoom;
    }

    private void OnJoinRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (1 <= RootManager.DataManager.Room.Count)
            {
                PhotonNetwork.JoinRandomRoom();
                PhotonNetwork.LoadLevel("WaitingRoomScene");
            }
            else
            {
                failedJoinRoomCanvas.SetActive(true);
            }
        }
    }
}
