using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private const string lobbyName = "Duck Duck Party";
    private const string gameVersion = "0.0.1";
    private int repeatTime = 1;

    [SerializeField] private GameObject touchGuide;
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        touchGuide.SetActive(true);
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        Debug.Log("서버에 연결되었습니다.");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();

        Debug.Log($"{cause}의 이유로 서버 연결에 실패하였습니다.");
    }

    public override void OnJoinedLobby()
    {
        if (0 < repeatTime)
        {
            repeatTime -= 1;
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.CurrentLobby.Name = lobbyName;
        Debug.Log("로비에 연결되었습니다.");
    }

    public override void OnLeftLobby()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("로비를 떠났습니다.");
    }
}
