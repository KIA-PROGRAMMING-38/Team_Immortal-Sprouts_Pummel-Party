using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private const string LOBBY_NAME = "Duck Duck Party";
    private const string GAME_VERSION = "0.0.1";
    private int repeatTime = 1;

    public UnityEvent OnConnectedToMasterServer = new UnityEvent();
    public UnityEvent OnJoinedNewRoom = new UnityEvent();
    public UnityEvent OnLeftTheRoom = new UnityEvent();


    private void Awake()
    {
        Managers.PhotonManager = this;
    }
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = GAME_VERSION;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        OnConnectedToMasterServer?.Invoke();
    }

    public override void OnDisconnected(DisconnectCause cause) => PhotonNetwork.ConnectUsingSettings();

    public override void OnJoinedLobby()
    {
        if (0 < repeatTime)
        {
            repeatTime -= 1;
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.CurrentLobby.Name = LOBBY_NAME;
    }

    public override void OnLeftLobby() => PhotonNetwork.JoinLobby();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo updatedRoomInfo in roomList)
        {
            string updatedRoomName = updatedRoomInfo.Name;
            bool isNewlyCreated;
            if (updatedRoomInfo.RemovedFromList) // 방이 삭제되었다면
            {
                isNewlyCreated = false;
                Managers.DataManager.Room.UpdateRoomData(isNewlyCreated, updatedRoomName);
            }
            else // 방이 삭제 된게 아니라면
            {
                if (!Managers.DataManager.Room.CheckIfRoomExist(updatedRoomName)) // 새로 생성된 방이라면
                {
                    isNewlyCreated = true;
                    Managers.DataManager.Room.UpdateRoomData(isNewlyCreated, updatedRoomName, updatedRoomInfo);
                }
            }
        }
    }

    public override void OnJoinedRoom() => OnJoinedNewRoom?.Invoke();

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        
    }

    public override void OnLeftRoom() => OnLeftTheRoom?.Invoke();
}
