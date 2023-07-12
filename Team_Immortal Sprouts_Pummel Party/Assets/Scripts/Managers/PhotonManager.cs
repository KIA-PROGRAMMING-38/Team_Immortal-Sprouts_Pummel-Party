using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System;



public class PhotonManager : MonoBehaviourPunCallbacks
{
    private const string LOBBY_NAME = "Duck Duck Party";
    private const string GAME_VERSION = "0.0.1";
    private int repeatTime = 1;

    public Dictionary<Player, PlayerData> Players { get; private set; } = new Dictionary<Player, PlayerData>();

    public void Init()
    {
        BindEvent(ref OnConnectedToMasterServer, () => PhotonNetwork.JoinLobby(), true);
        BindEvent(ref OnDisconnectedFromMasterServer, () => PhotonNetwork.ConnectUsingSettings(), true);
        BindEvent(ref OnJoinedTheLobby, RefreshServer, true);
        BindEvent(ref OnLeftTheLobby, () => PhotonNetwork.JoinLobby(), true);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = GAME_VERSION;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnDisable()
    {
        BindEvent(ref OnConnectedToMasterServer, () => PhotonNetwork.JoinLobby(), false);
        BindEvent(ref OnDisconnectedFromMasterServer, () => PhotonNetwork.ConnectUsingSettings(), false);
        BindEvent(ref OnJoinedTheLobby, RefreshServer, false);
        BindEvent(ref OnLeftTheLobby, () => PhotonNetwork.JoinLobby(), false);
    }

    #region EVENTS
    public event Action OnConnectedToMasterServer;
    public event Action OnDisconnectedFromMasterServer;
    public event Action OnJoinedTheLobby;
    public event Action OnLeftTheLobby;
    public event Action OnJoinedTheRoom;
    public event Action OnLeftTheRoom;
    public event Action<Player> OnPlayerEnteredTheRoom;
    public event Action<Player> OnPlayerLeftTheRoom;
    public event Action<Player> OnMasterSwitched;
    #endregion

    #region 포톤 콜백
    public override void OnConnectedToMaster() => OnConnectedToMasterServer?.Invoke();
    public override void OnDisconnected(DisconnectCause cause) => OnDisconnectedFromMasterServer?.Invoke();
    public override void OnJoinedLobby() => OnJoinedTheLobby?.Invoke();
    public override void OnLeftLobby() => OnLeftTheLobby?.Invoke();
    public override void OnJoinedRoom() => OnJoinedTheRoom?.Invoke();
    public override void OnPlayerEnteredRoom(Player newPlayer) => OnPlayerEnteredTheRoom?.Invoke(newPlayer);
    public override void OnPlayerLeftRoom(Player otherPlayer) => OnPlayerLeftTheRoom?.Invoke(otherPlayer);
    public override void OnMasterClientSwitched(Player newMasterClient) => OnMasterSwitched?.Invoke(newMasterClient);
    public override void OnLeftRoom() => OnLeftTheRoom?.Invoke();
    #endregion

    public void BindEvent(ref Action onEvent, Action function, bool isAdding)
    {
        if (isAdding)
        {
            onEvent -= function;
            onEvent += function;
        }
        else
        {
            onEvent -= function;
        }
    }

    private void RefreshServer()
    {
        if (0 < repeatTime) // 처음 접속 한번만 방목록 업데이트를 위해 2번 돌림
        {
            repeatTime -= 1;
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.CurrentLobby.Name = LOBBY_NAME;
        }
    }
}
