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
    [SerializeField] private GameObject[] playerSlots;
    [SerializeField] private ModelData modelData;
    [SerializeField] private PositionData positionData;

    private const string modelPrefabPath = "Prefabs/Lobby/WaitingRoomCanvas/";


    public void CanvasInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }


    #region Photon Callback �̺�Ʈ �Լ�
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient) // ������ ��쿡�� �÷��̾� ��ȯ
        {
            Debug.Log("���� ����?");
            SummonPlayerModel(PhotonNetwork.LocalPlayer.ActorNumber);
        }

        roomName.text = $"Room{PhotonNetwork.CurrentRoom.Name}";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) // ������ �ƴ� �ٸ� �÷��̾ ������ ��
    {
        if (PhotonNetwork.IsMasterClient) // ���常 ��ȯ���ټ� �ִ� ������ ��
        {
            Debug.Log($"�ٸ� �÷��̾ ���� = {newPlayer.ActorNumber}");
            SummonPlayerModel(newPlayer.ActorNumber);
        }
    }

    #endregion

    private void SummonPlayerModel(int playerActorNumber)
    {
        GameObject player = modelData._LobbyModels[playerActorNumber]; // ���� �����ؿ�
        Vector3 spawnPosition = positionData._LobbyPositions[playerActorNumber].position; // ��ȯ�� ������ �޾ƿ�
        Quaternion rotationValue = positionData._LobbyPositions[playerActorNumber].rotation; // ��ȯ�� ���� �޾ƿ�

        Debug.Log($"actornumber = {playerActorNumber}");
        PhotonNetwork.Instantiate($"{modelPrefabPath}{player.name}", spawnPosition, rotationValue);
    }

}
