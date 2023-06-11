using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingRoomCanvas : MonoBehaviourPunCallbacks
{
    private LobbyCanvases _lobbyCanvases;
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private ModelData modelData;
    [SerializeField] private PositionData positionData;
    [SerializeField] private PlayerSlot[] playerSlots;

    [SerializeField] private GameObject[] lobbyPlayerModels; // �׽�Ʈ ���� serializeFiled ����

    [SerializeField] private int playerPositionIndex = 1; // �׽�Ʈ ���� serializeFiled ����
    private Dictionary<string, int> playerDictionary = new Dictionary<string, int>();

    private const string modelPrefabPath = "Prefabs/Lobby/WaitingRoomCanvas/";


    public void CanvasInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }

    #region OnClick �̺�Ʈ �Լ�

    public void OnClick_LeaveButton()
    {
        LeaveCurrentRoom();
    }

    #endregion

    #region Photon Callback �̺�Ʈ �Լ�
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient) // ������ ��쿡�� �÷��̾� ��ȯ
        {
            Debug.Log("���� ����?");
            lobbyPlayerModels = new GameObject[PhotonNetwork.CurrentRoom.MaxPlayers + 1]; // 0��° �ε��� �Ⱦ�
            SummonPlayerModel(playerPositionIndex);
            AddPlayerData(PhotonNetwork.LocalPlayer.UserId, playerPositionIndex, PhotonNetwork.MasterClient);
        }

        roomName.text = $"Room{PhotonNetwork.CurrentRoom.Name}";
    }

    public override void OnMasterClientSwitched(Player newMasterClient) // ������ �ٲ������
    {
        // ������ ������ ���� �Ͷ߷����Ѵ�
        LeaveCurrentRoom(); // �׷��� �ٸ� �÷��̾�鵵 �� ������
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) // ������ �ƴ� �ٸ� �÷��̾ ������ ��
    {
        if (PhotonNetwork.IsMasterClient) // ���常 ��ȯ���ټ� �ִ� ������ ��
        {
            SummonPlayerModel(playerPositionIndex);
            PlayerSlot newPlayerSlot = playerSlots[playerPositionIndex];
            PhotonView newPlayerSlotPhotonView = PhotonView.Get(newPlayerSlot);
            newPlayerSlotPhotonView.RPC("EnableSelectCanvasButtons", RpcTarget.AllBuffered);
            AddPlayerData(newPlayer.UserId, playerPositionIndex, newPlayer);
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            DestroyPlayerModel(playerDictionary[otherPlayer.UserId]);
            DeletePlayerData(otherPlayer.UserId);
        }
    }

    #endregion

    private void AddPlayerData(string userID, int positionIndex, Player newPlayer)
    {
        playerDictionary.Add(userID, positionIndex);
        playerSlots[positionIndex].GetPhotonView().TransferOwnership(newPlayer);
        ++playerPositionIndex;
    }


    private void DeletePlayerData(string userID)
    {
        playerDictionary.Remove(userID);
        --playerPositionIndex;
    }

    private void LeaveCurrentRoom()
    {
        gameObject.SetActive(false);
        _lobbyCanvases.MultiGameCanvas.Active();
        _lobbyCanvases.MultiGameCanvas.TurnOnRaycast();
        playerPositionIndex = 1; // 1�� �ʱ�ȭ���ش�
        playerDictionary.Clear();
        playerSlots[1].ResetReadyCount();
        ResetReadyStatus();
        PhotonNetwork.LeaveRoom();
    }

    private void ResetReadyStatus()
    {
        for (int i = 2; i < playerSlots.Length; ++i)
        {
            playerSlots[i].GetSelectCanvas().ResetReadyStatus();
            playerSlots[i].SetNotReadyColor();
        }

        playerSlots[1].GetSelectCanvas().ResetStartButton(); // ������ StartButton interactable �� false�� �ʱ�ȭ
    }

    private void DestroyPlayerModel(int positionIndex)
    {
        if (lobbyPlayerModels[positionIndex] != null)
        {
            PhotonNetwork.Destroy(lobbyPlayerModels[positionIndex]);
            lobbyPlayerModels[positionIndex] = null; // ���� �÷��̾��� ���� ��ġ�� null�� �Ҵ����ش�
        }
    }

    private void SummonPlayerModel(int positionIndex)
    {
        GameObject player = modelData._LobbyModels[positionIndex]; // ���� �����ؿ�
        Vector3 spawnPosition = positionData._LobbyPositions[positionIndex].position; // ��ȯ�� ������ �޾ƿ�
        Quaternion rotationValue = positionData._LobbyPositions[positionIndex].rotation; // ��ȯ�� ���� �޾ƿ�

        GameObject summonedPlayer = PhotonNetwork.Instantiate($"{modelPrefabPath}{player.name}", spawnPosition, rotationValue);
        lobbyPlayerModels[positionIndex] = summonedPlayer; // ������ ���̸� ����ش�
    }

}
