using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private const string LOBBY_NAME = "Duck Duck Party";
    private const string GAME_VERSION = "0.0.1";
    private int repeatTime = 1;

    [SerializeField] private GameObject touchGuide;
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = GAME_VERSION;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        touchGuide.SetActive(true);
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
        if (0 < repeatTime)
        {
            repeatTime -= 1;
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.CurrentLobby.Name = LOBBY_NAME;
        Debug.Log("�κ� ����Ǿ����ϴ�.");
    }

    public override void OnLeftLobby()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("�κ� �������ϴ�.");
    }
}
