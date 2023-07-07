using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WaitingRoomPresenter : MonoBehaviourPunCallbacks
{
    private PhotonView presenterPV;
    [SerializeField] private CreatedRoomData createdRoomData; // 모델이 됌
    [field : SerializeField] public WaitingRoomView[] WaitingViews { get; set; }    

    private void OnEnable()
    {







        Managers.PhotonManager.OnLeftTheRoom.RemoveListener(loadLobby);
        Managers.PhotonManager.OnLeftTheRoom.AddListener(loadLobby);
    }

    private void OnDisable()
    {






        Managers.PhotonManager.OnLeftTheRoom.RemoveListener(loadLobby);
    }

    /// <summary>
    /// 주어진 입장순서에 해당하는 플레이어의 몸색을 초기화 해주는 함수
    /// </summary>
    /// <param name="enterOrder"></param>
    public void ResetBodyColor(int enterOrder) // 마스터가 해줌
    {
        if (createdRoomData.ModelPVs[enterOrder] != null)
        {
            createdRoomData.ModelPVs[enterOrder].RPC("SetBodyColor", RpcTarget.AllBuffered, enterOrder); // 바디 칼라 리셋
        }
    }

    /// <summary>
    /// 주어진 인덱스에 따라 UI상의 몸 배경색을 반환하는 함수
    /// </summary>
    /// <param name="colorIndex"></param>
    /// <returns></returns>
    public Color GetBackgroundColor(int colorIndex)
    {
        return createdRoomData.GetBackgroundColorData(colorIndex);
    }

    public string GetBackgroundHatText(int hatIndex)
    {
        return createdRoomData.GetBackgroundHatTextData(hatIndex);
    }


    /// <summary>
    /// 마스터에게 플레이어 몸색을 바꿔달라고 요청하는 함수
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="lastIndex"></param>
    /// <param name="wantBodyIndex"></param>
    /// <param name="isRightButton"></param>
    /// <param name="isFirstEntry"></param>
    [PunRPC]
    public void AskBodyColorUpdate(int enterOrder, int lastIndex, int wantBodyIndex, bool isRightButton, bool isFirstEntry) // 마스터 클라이언트에서 실행될 함수
    {
        wantBodyIndex = createdRoomData.GetCapableBodyIndex(lastIndex, wantBodyIndex, isRightButton, isFirstEntry);

        Player askedPlayer = Managers.DataManager.Player.GetPhotonPlayer(enterOrder);
        Managers.DataManager.Player.SetBodyID(askedPlayer, wantBodyIndex);

        createdRoomData.ModelPVs[enterOrder].RPC("SetBodyColor", RpcTarget.AllBuffered, wantBodyIndex); // 플레이어의 몸색깔을 바꿔줌
        WaitingViews[enterOrder].GetViewPV().RPC("UpdateBodyIndex", RpcTarget.AllBuffered, wantBodyIndex);

        WaitingViews[enterOrder].GetViewPV().RPC("SetBackgroundColor", askedPlayer, wantBodyIndex);
    }


    /// <summary>
    /// 마스터에게 모자를 바꿔달라고 요청하는 함수
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="hatIndex"></param>
    [PunRPC]
    public void AskHatUpdate(int enterOrder, int hatIndex) // 마스터 클라이언트에서 실행될 함수
    {
        //UpdateHatData(enterOrder, hatIndex); // 플레이어의 모자 데이터를 갱신해줌

        Player askedPlayer = Managers.DataManager.Player.GetPhotonPlayer(enterOrder);
        Managers.DataManager.Player.SetHatID(askedPlayer, hatIndex);

        createdRoomData.ModelPVs[enterOrder].RPC("SetHatOnPlayer", RpcTarget.AllBuffered, hatIndex); // 플레이어 모자를 바꿔줌

        Player askPlayer = Managers.DataManager.Player.GetPhotonPlayer(enterOrder);
        WaitingViews[enterOrder].GetViewPV().RPC("SetHatText", askPlayer, hatIndex);
    }


    /// <summary>
    /// 데이터에 플레이어 몸색 인덱스 정보를 업데이트 하는 함수
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="bodyIndex"></param>
    private void UpdateBodyData(int enterOrder, int bodyIndex) // 데이터를 갱신하기 위해 마스터 클라이언트만 접근할 함수
    {
        Player updatePlayer = Managers.DataManager.Player.GetPhotonPlayer(enterOrder);

        createdRoomData.UpdateBodyIndex(updatePlayer, bodyIndex);

    }

    /// <summary>
    /// 데이터에 플레이어 모자 인덱스 정보를 업데이트 하는 함수
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="hatIndex"></param>
    private void UpdateHatData(int enterOrder, int hatIndex) // 데이터를 갱신하기 위해 마스터 클라이언트만 접근할 함수
    {
        Player updatePlayer = Managers.DataManager.Player.GetPhotonPlayer(enterOrder);

        createdRoomData.UpdateHatIndex(updatePlayer, hatIndex);
    }

    public PhotonView GetPresenterPV()
    {
        if (presenterPV == null)
        {
            presenterPV = GetComponent<PhotonView>();
        }

        return presenterPV;
    }


    /// <summary>
    /// 마스터가 주어진 입장순서의 플레이어를 레디시키는 함수
    /// </summary>
    /// <param name="enterOrder"></param>
    [PunRPC]
    public void SetReady(int enterOrder) // 마스터 클라이언트만 실행시켜줄 함수
    {
        if (createdRoomData.IsPlayerPresent[enterOrder] == true)
        {
            if (createdRoomData.IsReady[enterOrder] == true)
            {
                createdRoomData.IsReady[enterOrder] = false;
                --createdRoomData.ReadyCount;
            }
            else // 레디를 안한 상태라면
            {
                createdRoomData.IsReady[enterOrder] = true;
                ++createdRoomData.ReadyCount;
            }

            WaitingViews[enterOrder].GetViewPV().RPC("SetReadyColor", RpcTarget.AllBuffered, createdRoomData.IsReady[enterOrder]);
        }

        CheckIfStartable();
    }

    /// <summary>
    /// 게임 시작이 가능한지 판단 후, 시작버튼을 활성화 하는 함수
    /// </summary>
    /// <param name="readyCount"></param>
    private void CheckIfStartable()
    {
        createdRoomData.UpdateStartable();

        if (createdRoomData.IsStartable == true)
        {
            WaitingViews[1].GetViewPV().RPC("ActivateStartButton", RpcTarget.All, createdRoomData.IsStartable); // 방장의 StartButton 활성화
        }
    }

    #region Photon 콜백 함수들
    //public override void OnJoinedRoom()
    //{
    //    createdRoomData.IsOriginalMaster = PhotonNetwork.IsMasterClient;

    //    if (createdRoomData.IsOriginalMaster)
    //    {
    //        //int enterOrder = 1;
    //        //createdRoomData.IsPlayerPresent[enterOrder] = true;
    //        //Player localPlayer = PhotonNetwork.LocalPlayer;

    //        //Managers.DataManager.Player.UpdatePlayerData(localPlayer, enterOrder);
    //        //Managers.DataManager.Player.SetNickName(localPlayer, createdRoomData.GetDefaultName(enterOrder));
    //        //Managers.DataManager.Player.SetBodyID(localPlayer, enterOrder);
    //        //Managers.DataManager.Player.SetHatID(localPlayer, 0);

    //        // 플레이어 생성
    //        //GameObject model = Managers.PrefabManager.Instantiate("RoomWait", createdRoomData.PositionTransforms[enterOrder].position, createdRoomData.PositionTransforms[enterOrder].rotation);
    //        //model.SetActive(true);

    //        //PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // 모델체인저 뽑아옴
    //        //createdRoomData.ModelChangers[enterOrder] = modelChanger; // 모델 체인저 저장해둠 --> 마스터가 다 컨트롤할라구
    //        //createdRoomData.ModelPVs[enterOrder] = PhotonView.Get(model); // 모델체인저와 연동된 포톤뷰 저장해둠 -> 마스터가 다 컨트롤하라구
    //        //createdRoomData.ModelPVs[enterOrder].ViewID = enterOrder * 999; // 포톤뷰 ID를 메뉴얼하게 설정해줘야한다고 함....

    //        //waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder); // View 의 입장순서를 업데이트해줌

    //        //AskBodyColorUpdate(enterOrder, enterOrder, enterOrder, true, true); // 색을 바꿔줌
    //        //++createdRoomData.PlayerCount;
    //    }
    //}

    public void UpdateNewPlayerData()
    {
        createdRoomData.IsOriginalMaster = PhotonNetwork.IsMasterClient;

        if (createdRoomData.IsOriginalMaster)
        {
            int enterOrder = 1;
            createdRoomData.IsPlayerPresent[enterOrder] = true;
            Player localPlayer = PhotonNetwork.LocalPlayer;
            Managers.DataManager.Player.UpdatePlayerData(localPlayer, enterOrder);
            Managers.DataManager.Player.SetNickName(localPlayer, createdRoomData.GetDefaultName(enterOrder));
            Managers.DataManager.Player.SetBodyID(localPlayer, enterOrder);
            Managers.DataManager.Player.SetHatID(localPlayer, 0);

            // 플레이어 생성
            GameObject model = Managers.PrefabManager.Instantiate("RoomWait", createdRoomData.PositionTransforms[enterOrder].position, createdRoomData.PositionTransforms[enterOrder].rotation);
            model.SetActive(true);

            PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // 모델체인저 뽑아옴
            createdRoomData.ModelChangers[enterOrder] = modelChanger; // 모델 체인저 저장해둠 --> 마스터가 다 컨트롤할라구
            createdRoomData.ModelPVs[enterOrder] = PhotonView.Get(model); // 모델체인저와 연동된 포톤뷰 저장해둠 -> 마스터가 다 컨트롤하라구
            createdRoomData.ModelPVs[enterOrder].ViewID = enterOrder * 999; // 포톤뷰 ID를 메뉴얼하게 설정해줘야한다고 함....

            WaitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder); // View 의 입장순서를 업데이트해줌

            AskBodyColorUpdate(enterOrder, enterOrder, enterOrder, true, true); // 색을 바꿔줌
            ++createdRoomData.PlayerCount;
        }


    }


    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    //if (PhotonNetwork.IsMasterClient)
    //    //{
    //    //    int enterOrder = createdRoomData.GetEmptySlot(); // 빈자리를 찾아서, 입장순서를 정해줌
    //    //    createdRoomData.IsPlayerPresent[enterOrder] = true;

    //    //    Managers.DataManager.Player.UpdatePlayerData(newPlayer, enterOrder);
    //    //    Managers.DataManager.Player.SetNickName(newPlayer, createdRoomData.GetDefaultName(enterOrder));
    //    //    Managers.DataManager.Player.SetBodyID(newPlayer, enterOrder);
    //    //    Managers.DataManager.Player.SetHatID(newPlayer, 0);

    //    //    // 플레이어 생성
    //    //    GameObject model = Managers.PrefabManager.Instantiate("RoomWait", createdRoomData.PositionTransforms[enterOrder].position, createdRoomData.PositionTransforms[enterOrder].rotation);
    //    //    model.SetActive(true);

    //    //    PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // 모델 체인저 뽑아옴
    //    //    createdRoomData.ModelChangers[enterOrder] = modelChanger; // 모델체인저 저장해둠 --> 마스터가 다 조종할라고
    //    //    createdRoomData.ModelPVs[enterOrder] = PhotonView.Get(model); // 모델체인저와 연동된 포톤뷰 저장해둠 -> 마스터가 다 컨트롤하라구


    //    //    // View 에 입장순서 할당해줌
    //    //    WaitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder);
    //    //    WaitingViews[enterOrder].GetViewPV().TransferOwnership(newPlayer); // 소유권 양도해줌

    //    //    AskBodyColorUpdate(enterOrder, enterOrder, enterOrder, true, true); // 색을 바꿔줌
    //    //    ++createdRoomData.PlayerCount;
    //    //    createdRoomData.CheckIfRoomStillOpen();
    //    //}
    //}

    public void UpdateNewOtherPlayerData(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int enterOrder = createdRoomData.GetEmptySlot(); // 빈자리를 찾아서, 입장순서를 정해줌
            createdRoomData.IsPlayerPresent[enterOrder] = true;

            Managers.DataManager.Player.UpdatePlayerData(newPlayer, enterOrder);
            Managers.DataManager.Player.SetNickName(newPlayer, createdRoomData.GetDefaultName(enterOrder));
            Managers.DataManager.Player.SetBodyID(newPlayer, enterOrder);
            Managers.DataManager.Player.SetHatID(newPlayer, 0);

            // 플레이어 생성
            GameObject model = Managers.PrefabManager.Instantiate("RoomWait", createdRoomData.PositionTransforms[enterOrder].position, createdRoomData.PositionTransforms[enterOrder].rotation);
            model.SetActive(true);

            PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // 모델 체인저 뽑아옴
            createdRoomData.ModelChangers[enterOrder] = modelChanger; // 모델체인저 저장해둠 --> 마스터가 다 조종할라고
            createdRoomData.ModelPVs[enterOrder] = PhotonView.Get(model); // 모델체인저와 연동된 포톤뷰 저장해둠 -> 마스터가 다 컨트롤하라구
            createdRoomData.ModelPVs[enterOrder].ViewID = enterOrder * 999; // 포톤뷰 ID를 메뉴얼하게 설정해줘야한다고 함....

            // View 에 입장순서 할당해줌
            WaitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder);
            WaitingViews[enterOrder].GetViewPV().TransferOwnership(newPlayer); // 소유권 양도해줌

            AskBodyColorUpdate(enterOrder, enterOrder, enterOrder, true, true); // 색을 바꿔줌
            ++createdRoomData.PlayerCount;
            createdRoomData.CheckIfRoomStillOpen();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (createdRoomData.IsOriginalMaster && PhotonNetwork.IsMasterClient) // 마스터가해줘야 할 일들
        {
            int leftPlayerEnterOrder = Managers.DataManager.Player.GetEnterOrder(otherPlayer);
            ResetBodyColor(leftPlayerEnterOrder);
            DestroyOtherPlayer(leftPlayerEnterOrder);
            createdRoomData.IsPlayerPresent[leftPlayerEnterOrder] = false;
            createdRoomData.colorIndexing[Managers.DataManager.Player.GetBodyID(otherPlayer)] = false;

            // 플레이어가 나갔을때 기본값으로 돌려주는 부분
            createdRoomData.IsReady[leftPlayerEnterOrder] = false;
            WaitingViews[leftPlayerEnterOrder].GetViewPV().RPC("SetReadyColor", RpcTarget.AllBuffered, false);
            --createdRoomData.ReadyCount;
            CheckIfStartable();
            WaitingViews[leftPlayerEnterOrder].GetViewPV().RPC("ShowPlayerNickName", RpcTarget.AllBuffered, createdRoomData.GetDefaultName(leftPlayerEnterOrder));

            createdRoomData.CheckIfRoomStillOpen();
            Managers.DataManager.Player.RemovePlayerData(leftPlayerEnterOrder);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // 마스터가 바뀌었다 -> 방이 폭파된다 -> 다 나가
        PhotonNetwork.LeaveRoom();
    }

    private void loadLobby() => PhotonNetwork.LoadLevel(0); // 이거도 LobbyManager가 해줘야함

    #endregion


    public void MoveToBoardGame() // 이것도 나중에 로드매니저가 해줘야함
    {
        if (PhotonNetwork.IsMasterClient)
        {
            WrapObject.SCENE_CHANGER.BoardGameLoadPlay();
            PhotonNetwork.LoadLevel(2);
        }
    }

    private void KickEveryoneOut()
    {
        // 방장부터 나가면 MasterClient가 옮겨가기 때문에, 이렇게 처리함
        for (int enterOrder = createdRoomData.IsPlayerPresent.Length - 1; 0 < enterOrder; --enterOrder)
        {
            DestroyOtherPlayer(enterOrder);
        }
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void DestroyOtherPlayer(int enterOrder) // 마스터만 접근할 함수
    {
        if (createdRoomData.ModelChangers[enterOrder] != null)
        {
            GameObject hat = createdRoomData.ModelChangers[enterOrder].GetCurrentHat();

            if (hat != null)
            {
                createdRoomData.ModelChangers[enterOrder].RemoveCurrentHat();
            }
        }

        PhotonView photonView = createdRoomData.ModelPVs[enterOrder];
        if (photonView != null)
        {
            PhotonNetwork.Destroy(photonView); // 게임캐릭터 제거
        }
    }


    public void OnClick_LeaveRoom() // Leave 버튼에 들어갈 함수 => Public 이여야 함
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int enterOrder = 1; enterOrder <= PhotonNetwork.CurrentRoom.PlayerCount; ++enterOrder)
            {
                ResetBodyColor(enterOrder);
            }
            KickEveryoneOut();
        }
        LeaveRoom();
    }

    /// <summary>
    /// 플레이어의 닉네임을 데이터에 업데이트하고, 다른 플레이어들에게 보여주는 함수
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="newNickName"></param>
    [PunRPC]
    public void SetPlayerNickName(int enterOrder, string newNickName)
    {
        Player updatePlayer = Managers.DataManager.Player.GetPhotonPlayer(enterOrder);
        createdRoomData.UpdateNickName(updatePlayer, newNickName);
        WaitingViews[enterOrder].GetViewPV().RPC("ShowPlayerNickName", RpcTarget.AllBuffered, newNickName);
    }

}
