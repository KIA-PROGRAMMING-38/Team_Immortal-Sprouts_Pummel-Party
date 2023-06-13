using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingRoomCanvas : MonoBehaviourPunCallbacks
{
    private LobbyCanvases _lobbyCanvases;
    [Header("Data Scriptable Object")]
    [SerializeField] private ModelData modelData;
    [SerializeField] private PositionData positionData;


    [Header("-------------------------------------------------------------------------------")]
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private PlayerSlot[] playerSlots;

    //[SerializeField] private GameObject[] lobbyPlayerModels; // 테스트 위해 serializeFiled 부착
    [SerializeField] private Texture2D[] modelBodyTextures;
    
    [SerializeField] private PlayerModelChanger[] playerModelChangers;

    

    private int playerPositionIndex;
    private Dictionary<string, int> playerDictionary = new Dictionary<string, int>();
    private bool[] isPlayerPresent = new bool[5]; 

    private const string modelPrefabPath = "Prefabs/Lobby/WaitingRoomCanvas/";

    public PlayerSlot GetPlayerSlot(int index)
    {
        return playerSlots[index];
    }

    public PlayerModelChanger GetPlayerModelChanger(int playerIndex)
    {
        return playerModelChangers[playerIndex];
    }

    public void CanvasInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }

    #region OnClick 이벤트 함수

    public void OnClick_LeaveButton()
    {
        LeaveCurrentRoom();
    }

    #endregion

   


    #region Photon Callback 이벤트 함수
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient) // 방장일 경우에만 플레이어 소환
        {
            playerPositionIndex = CheckEmptySlot();
            AddPlayerData(PhotonNetwork.LocalPlayer.UserId, playerPositionIndex, PhotonNetwork.MasterClient);
            playerSlots[playerPositionIndex].GetCustomizeCanvas().SetMasterCustomizeCanvasPlayerIndex(playerPositionIndex);
            isPlayerPresent[playerPositionIndex] = true;
        }

        roomName.text = $"Room{PhotonNetwork.CurrentRoom.Name}";
    }

    public override void OnMasterClientSwitched(Player newMasterClient) // 방장이 바뀌었을때
    {
        // 방장이 나가면 방을 터뜨려야한다
        LeaveCurrentRoom(); // 그래서 다른 플레이어들도 다 나간다
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) // 방장이 아닌 다른 플레이어가 들어왔을 때
    {
        PhotonView.Get(playerSlots[playerPositionIndex].GetCustomizeCanvas()).RPC("SetCustomizeCanvasPlayerIndex", RpcTarget.All, playerPositionIndex);
        Debug.Log("누군가 들어옴");
        if (PhotonNetwork.IsMasterClient) // 방장만 소환해줄수 있는 권한을 줌
        {
            PlayerSlot newPlayerSlot = playerSlots[playerPositionIndex];
            PhotonView newPlayerSlotPhotonView = PhotonView.Get(newPlayerSlot);
            newPlayerSlotPhotonView.RPC("EnableSelectCanvasButtons", RpcTarget.AllBuffered);
            playerPositionIndex = CheckEmptySlot();
            //SummonPlayerModel(playerPositionIndex, newPlayer);
            AddPlayerData(newPlayer.UserId, playerPositionIndex, newPlayer); // 플레이어 슬롯 양도권 넘겨주고
            isPlayerPresent[playerPositionIndex] = true;

            TransferPhotonViews(newPlayer).Forget();
            //PhotonView.Get(gameObject).RPC("TurnOnPlayerModel", newPlayer, true, playerPositionIndex);
            TurnOnMultiplePlayers(PhotonView.Get(gameObject), newPlayer, "TurnOnPlayerModel", isPlayerPresent);
        }
    }


    private void TurnOnMultiplePlayers(PhotonView photonView, Player targetPlayer, string nameOfRPCFunction, bool[] playerPresence)
    {
        for (int i = 1; i < playerPresence.Length ;++i)
        {
            if (playerPresence[i] == true)
            {
                photonView.RPC(nameOfRPCFunction, targetPlayer ,true, i);
            }
        }
    }

    [PunRPC]
    private void TurnOnPlayerModel(bool isTurnOn, int playerPositionIndex)
    {
        playerModelChangers[playerPositionIndex].gameObject.SetActive(isTurnOn);
    }

    private async UniTaskVoid TransferPhotonViews(Player newPlayer)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(5f));
        PhotonView.Get(playerSlots[playerPositionIndex].GetCustomizeCanvas()).TransferOwnership(newPlayer); // 커스터마이즈 캔버스 양도권 넘겨준다
        PhotonView.Get(playerModelChangers[playerPositionIndex]).TransferOwnership(newPlayer); // 플레이어 모델 체이저 양도권 넘겨주고
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // otherPlayer에서 쓸만한게 userID밖에 없음
            // userID는 고정이라, 여기에 해당하는 포지션 인덱스의 게임오브젝트를 파괴한다
            //DestroyPlayerModel(playerDictionary[otherPlayer.UserId]); 
            DeletePlayerData(otherPlayer.UserId);
            PhotonView.Get(gameObject).RPC("TurnOnPlayerModel", RpcTarget.All, false, playerPositionIndex);
            isPlayerPresent[playerPositionIndex] = false;
        }
    }

    #endregion

    private int CheckEmptySlot()
    {
        int positionIndex = 0;

        for (int i = 1; i < playerSlots.Length; ++i)
        {
            if (isPlayerPresent[i] == false)
            {
                positionIndex = i;
                break;
            }
        }

        return positionIndex;
    }

    private void AddPlayerData(string userID, int positionIndex, Player newPlayer)
    {
        playerDictionary.Add(userID, positionIndex);
        playerSlots[positionIndex].GetPhotonView().TransferOwnership(newPlayer);
    }


    private void DeletePlayerData(string userID)
    {
        playerDictionary.Remove(userID);
    }

    private void LeaveCurrentRoom()
    {
        gameObject.SetActive(false);
        _lobbyCanvases.MultiGameCanvas.Active();
        _lobbyCanvases.MultiGameCanvas.TurnOnRaycast();
        playerPositionIndex = 1; // 1로 초기화해준다
        playerDictionary.Clear();
        playerSlots[1].ResetReadyCount();
        ResetPlayerPresentData();
        ResetReadyStatus();
        PhotonNetwork.LeaveRoom();
    }

    private void ResetPlayerPresentData()
    {
        for (int i = 0; i < isPlayerPresent.Length ;++i)
        {
            isPlayerPresent[i] = false;
        }
    }

    private void ResetReadyStatus()
    {
        for (int i = 2; i < playerSlots.Length; ++i)
        {
            playerSlots[i].GetSelectCanvas().ResetReadyStatus();
            playerSlots[i].SetNotReadyColor();
        }

        playerSlots[1].GetSelectCanvas().ResetStartButton(); // 방장의 StartButton interactable 을 false로 초기화
    }

    //private void DestroyPlayerModel(int positionIndex)
    //{
    //    if (lobbyPlayerModels[positionIndex] != null)
    //    {
    //        PhotonNetwork.Destroy(lobbyPlayerModels[positionIndex]);
    //        lobbyPlayerModels[positionIndex] = null; // 나간 플레이어의 모델의 위치에 null을 할당해준다
    //        isPlayerPresent[positionIndex] = false; // 플레이어가 나갔다는것을 표시한다
    //    }
    //}

    //private void SummonPlayerModel(int positionIndex, Player newPlayer)
    //{
    //    GameObject player = modelData._LobbyModels[positionIndex]; // 모델을 추출해옴
    //    Vector3 spawnPosition = positionData._LobbyPositions[positionIndex].position; // 소환할 포지션 받아옴
    //    Quaternion rotationValue = positionData._LobbyPositions[positionIndex].rotation; // 소환할 각도 받아옴

    //    GameObject summonedPlayer = PhotonNetwork.Instantiate($"{modelPrefabPath}{player.name}", spawnPosition, rotationValue);
    //    summonedPlayer.transform.parent = this.transform;
    //    lobbyPlayerModels[positionIndex] = summonedPlayer; // 생성한 아이를 담아준다
    //    isPlayerPresent[positionIndex] = true; // 플레이어가 존재한다는 것을 표시한다
    //    PhotonView playerModelChangerPhotonView = PhotonView.Get(summonedPlayer);
    //    playerModelChangerPhotonView.TransferOwnership(newPlayer); // PlayerModelChanger의 pv의 양도권을 새로운 플레이어에게 넘겨준다


    //    playerSlots[positionIndex].GetCustomizeCanvas().SetMasterCustomizeCanvasPlayerIndex(positionIndex); // 방장이 알 수 있게끔
    //    if (newPlayer != PhotonNetwork.MasterClient)
    //    {
    //        PhotonView customizeCanvasPhotonView = PhotonView.Get(playerSlots[positionIndex].GetCustomizeCanvas());
    //        customizeCanvasPhotonView.TransferOwnership(newPlayer); // CustomizeCanvas의 pv 양도권을 새로운 플레이어에게 넘겨준다
    //        customizeCanvasPhotonView.RPC("SetCustomizeCanvasPlayerIndex", newPlayer, positionIndex); // 지가 몇번째 포지션에 있는 놈인지 알 수 있게끔
    //    }


    //    PlayerModelChanger playerModelChanger = summonedPlayer.GetComponent<PlayerModelChanger>();
    //    playerModelChangers[positionIndex] = playerModelChanger;
    //    playerSlots[positionIndex].SetPlayerModelChanger(playerModelChanger);
    //    // 다른 플레이어의 PlayerSlot의 자기 캐릭터의 PlayerModelChanger를 할당해줘야함...
    //}

    //private void UpdatePlayerModelChangers()
    //{

    //    for (int i = 0; i < 4 ;++i)
    //    {
    //        PlayerModelChanger playerModelChanger = GameObject.FindWithTag("LobbyPlayer").GetComponent<PlayerModelChanger>();
    //        if (playerModelChanger != null)
    //        {
    //            playerModelChangers[i] = playerModelChanger;
    //        }

    //    }
    //}
}
