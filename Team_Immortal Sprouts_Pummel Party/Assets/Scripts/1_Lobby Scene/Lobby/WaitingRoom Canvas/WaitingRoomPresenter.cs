using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomPresenter : MonoBehaviourPunCallbacks, IPunObservable
{
    private string roomName;
    [SerializeField] private TMP_Text roomNameText;

    private PhotonView presenterPV;
    [SerializeField] private LobbyPlayerData playerData; // 모델이 됌

    [SerializeField] private WaitingRoomView[] waitingViews;

    [SerializeField] private PositionData positionData;
    [SerializeField] private CustomData customData;


    [SerializeField] private int enterOrder = 1; // 테스트 위해 넣었음
    [SerializeField] private bool[] isReady = new bool[5];
    [SerializeField] private bool[] isPlayerPresent = new bool[5];

    [SerializeField] private GameObject[] playerModels;
    [SerializeField] private PlayerModelChanger[] modelChangers = new PlayerModelChanger[5];
    [SerializeField] private PhotonView[] modelPVs = new PhotonView[5];

    [SerializeField] private Player[] players = new Player[5];

    public int hatTypeCount { get; private set; }
    public int bodyColorCount { get; private set; }



    [PunRPC]
    public void AskBodyColorUpdate(int enterOrder, int lastIndex, int wantBodyIndex, bool isRightButton) // 마스터 클라이언트에서 실행될 함수
    {
        wantBodyIndex = playerData.GetCapableBodyIndex(lastIndex, wantBodyIndex, isRightButton);
            
        UpdateBodyData(enterOrder, wantBodyIndex); // 플레이어의 몸색깔 데이터를 갱신해줌

        modelPVs[enterOrder].RPC("SetBodyColor", RpcTarget.AllBuffered, wantBodyIndex); // 플레이어의 몸색깔을 바꿔줌
        waitingViews[enterOrder].GetViewPV().RPC("UpdateBodyIndex", RpcTarget.AllBuffered, wantBodyIndex);
    }

    [PunRPC]
    public void AskHatUpdate(int enterOrder, int hatIndex) // 마스터 클라이언트에서 실행될 함수
    {
        UpdateHatData(enterOrder, hatIndex); // 플레이어의 모자 데이터를 갱신해줌

        modelPVs[enterOrder].RPC("SetHatOnPlayer", RpcTarget.AllBuffered, hatIndex); // 플레이어 모자를 바꿔줌
    }

    private void UpdateBodyData(int enterOrder, int bodyIndex) // 데이터를 갱신하기 위해 마스터 클라이언트만 접근할 함수
    {
        Player updatePlayer = players[enterOrder];

        playerData.UpdateBodyIndex(updatePlayer, bodyIndex);

    }

    private void UpdateHatData(int enterOrder, int hatIndex) // 데이터를 갱신하기 위해 마스터 클라이언트만 접근할 함수
    {
        Player updatePlayer = players[enterOrder];

        playerData.UpdateHatIndex(updatePlayer, hatIndex);
    }

    public PhotonView GetPresenterPV()
    {
        if (presenterPV == null)
        {
            presenterPV = GetComponent<PhotonView>();
        }

        return presenterPV;
    }

    public PhotonView GetMasterPV()
    {
        return waitingViews[1].GetViewPV();
    }

    [PunRPC]
    public void SetReady(int enterOrder)
    {
        if (isReady[enterOrder] == true) // 이미 레디한 상태라면
        {
            // 레디를 해제한다
            isReady[enterOrder] = false;
            waitingViews[enterOrder].GetViewPV().RPC("SetReadyColor", RpcTarget.All, isReady[enterOrder]);
        }
        else
        {
            // 레디한다
            isReady[enterOrder] = true;
            waitingViews[enterOrder].GetViewPV().RPC("SetReadyColor", RpcTarget.All, isReady[enterOrder]);
        }
    }

    #region Photon 콜백 함수들
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isPlayerPresent[enterOrder] = true; // 들어옴 체크
            playerData.AddPlayerData(PhotonNetwork.LocalPlayer, enterOrder, $"Player {enterOrder}", enterOrder, 0); // Model(data) 업데이트
            playerData.UpdateColorIndexing(enterOrder);
            // 플레이어 생성
            GameObject model = PhotonNetwork.Instantiate($"Prefabs/Lobby/WaitingRoomCanvas/RoomWait {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);

            PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // 모델체인저 뽑아옴
            modelChangers[enterOrder] = modelChanger; // 모델 체인저 저장해둠 --> 마스터가 다 컨트롤할라구
            modelPVs[enterOrder] = PhotonView.Get(model); // 모델체인저와 연동된 포톤뷰 저장해둠 -> 마스터가 다 컨트롤하라구

            waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder); // View 의 입장순서를 업데이트해줌

            players[enterOrder] = PhotonNetwork.LocalPlayer;
        }

        hatTypeCount = customData.hats.Length;
        bodyColorCount = customData.bodyColors.Length;
        roomName = PhotonNetwork.CurrentRoom.Name;
        roomNameText.text = roomName;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            enterOrder = GetEmptySlot(); // 빈자리를 찾아서, 입장순서를 정해줌
            isPlayerPresent[enterOrder] = true; // 존재함 체크
            playerData.AddPlayerData(newPlayer, enterOrder, $"Player {enterOrder}", enterOrder, 0); // Model 업데이트
            playerData.UpdateColorIndexing(enterOrder);

            // 플레이어 생성
            GameObject model = PhotonNetwork.Instantiate($"Prefabs/Lobby/WaitingRoomCanvas/RoomWait {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);

            PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // 모델 체인저 뽑아옴
            modelChangers[enterOrder] = modelChanger; // 모델체인저 저장해둠 --> 마스터가 다 조종할라고
            modelPVs[enterOrder] = PhotonView.Get(model); // 모델체인저와 연동된 포톤뷰 저장해둠 -> 마스터가 다 컨트롤하라구


            // View 에 입장순서 할당해줌
            waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder);
            waitingViews[enterOrder].GetViewPV().TransferOwnership(newPlayer); // 소유권 양도해줌

            players[enterOrder] = newPlayer;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isPlayerPresent[playerData.GetPlayerEnterOrder(otherPlayer)] = false; // 나감 표시
            playerData.RemovePlayerData(otherPlayer); // MoDEL 업데이트
            //PhotonNetwork.Destroy(modelPVs[playerData.GetPlayerEnterOrder(otherPlayer)]); // 게임캐릭터 제거
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // 마스터가 바뀌었다 -> 방이 폭파된다 -> 다 나가
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby Scene 2");
    }


    #endregion

    [PunRPC]
    public void KickEveryeoneOut()
    {
        for (int enterOrder = isPlayerPresent.Length -1; 0 < enterOrder ;--enterOrder)
        {
            DestroyOtherPlayer(enterOrder);
        }
    }

    [PunRPC]
    public void MakePlayerLeave(int enterOrder)
    {
        DestroyOtherPlayer(enterOrder);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }



    
    public void DestroyOtherPlayer(int enterOrder) // 마스터만 접근할 함수
    {
        PhotonView photonView= modelPVs[enterOrder];
        if (photonView != null)
        {
            PhotonNetwork.Destroy(photonView); // 게임캐릭터 제거
        }

        if (modelChangers[enterOrder] != null)
        {
            GameObject hat = modelChangers[enterOrder].GetCurrentHat();
            
            if (hat != null)
            {
                PhotonNetwork.Destroy(PhotonView.Get(hat));
            }
        }
    }


    private int GetEmptySlot()
    {
        int emptyIndex = -9999;
        for (int i = 1; i < isPlayerPresent.Length; ++i)
        {
            if (isPlayerPresent[i] == false)
            {
                emptyIndex = i;
                return emptyIndex;
            }
        }

        return emptyIndex;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            for (int i = 1; i < isPlayerPresent.Length; ++i)
            {
                stream.SendNext(isPlayerPresent[i]);
            }
        }
        else if (stream.IsReading)
        {
            for (int i = 1; i < isPlayerPresent.Length; ++i)
            {
                isPlayerPresent[i] = (bool)stream.ReceiveNext();
            }
        }
    }



}
