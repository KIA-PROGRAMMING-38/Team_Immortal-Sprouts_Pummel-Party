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


    #region Photon Callback 이벤트 함수
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient) // 방장일 경우에만 플레이어 소환
        {
            Debug.Log("방장 들어옴?");
            SummonPlayerModel(PhotonNetwork.LocalPlayer.ActorNumber);
        }

        roomName.text = $"Room{PhotonNetwork.CurrentRoom.Name}";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) // 방장이 아닌 다른 플레이어가 들어왔을 때
    {
        if (PhotonNetwork.IsMasterClient) // 방장만 소환해줄수 있는 권한을 줌
        {
            Debug.Log($"다른 플레이어가 들어옴 = {newPlayer.ActorNumber}");
            SummonPlayerModel(newPlayer.ActorNumber);
        }
    }

    #endregion

    private void SummonPlayerModel(int playerActorNumber)
    {
        GameObject player = modelData._LobbyModels[playerActorNumber]; // 모델을 추출해옴
        Vector3 spawnPosition = positionData._LobbyPositions[playerActorNumber].position; // 소환할 포지션 받아옴
        Quaternion rotationValue = positionData._LobbyPositions[playerActorNumber].rotation; // 소환할 각도 받아옴

        Debug.Log($"actornumber = {playerActorNumber}");
        PhotonNetwork.Instantiate($"{modelPrefabPath}{player.name}", spawnPosition, rotationValue);
    }

}
