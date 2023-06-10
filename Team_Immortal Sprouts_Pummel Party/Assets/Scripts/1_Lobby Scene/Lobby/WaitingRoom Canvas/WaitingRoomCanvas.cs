using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerSlots;
    [SerializeField] private ModelData modelData;
    [SerializeField] private PositionData positionData;

    private const string modelPrefabPath = "Prefabs/Lobby/WaitingRoomCanvas/";

    private void OnEnable() // FindRoomCanvas ���� �̹� OnJoinedRoom()�� ����ϱ� ������ OnEnable�� ��ü��
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("����?");
            SummonPlayerModel(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }



    private void SummonPlayerModel(int playerActorNumber)
    {
        GameObject player = modelData._LobbyModels[playerActorNumber];
        Vector3 spawnPosition = positionData._LobbyPositions[playerActorNumber].position;
        Quaternion rotationValue = positionData._LobbyPositions[playerActorNumber].rotation;

        PhotonNetwork.Instantiate($"{modelPrefabPath}{player.name}", spawnPosition, rotationValue);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SummonPlayerModel(newPlayer.ActorNumber);
    }

}
