using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private const string lobbyName = "Duck Duck Party";
    private const string gameVersion = "0.0.1";

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        Debug.Log("������ ����Ǿ����ϴ�.");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();

        Debug.Log($"{cause}�� ������ ���� ���ῡ �����Ͽ����ϴ�.");
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.CurrentLobby.Name = lobbyName;
        Debug.Log("�κ� ����Ǿ����ϴ�.");
    }

    public override void OnLeftLobby()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("�κ� �������ϴ�.");
    }
}