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

    private Color[] colors;
    private string[] hatTexts;

    public int hatTypeCount { get; private set; }
    public int bodyColorCount { get; private set; }
    private bool amIOriginalMaster = false;

    private string[] defaultNames = new string[5];

    public void ResetBodyColor(int enterOrder) // 마스터가 해줌
    {
        if (modelPVs[enterOrder] != null)
        {
            modelPVs[enterOrder].RPC("SetBodyColor", RpcTarget.AllBuffered, enterOrder); // 바디 칼라 리셋
        }
    }

    private void InitBackgrounds()
    {
        colors = new Color[bodyColorCount];
        colors[0] = Color.black;
        colors[1] = Color.white;
        colors[2] = new Color(255f / 255f, 160f / 255f, 219f / 255f); // 핑크
        colors[3] = Color.green;
        colors[4] = Color.red;
        colors[5] = Color.blue;
        colors[6] = new Color(210f / 255f, 122f / 255f, 79f / 255f); // 연갈색
        colors[7] = Color.yellow;

        hatTexts = new string[hatTypeCount];
        hatTexts[0] = "None";
        hatTexts[1] = "Ribbon";
        hatTexts[2] = "BirthDay";
        hatTexts[3] = "Cap";
        hatTexts[4] = "Flower";
        hatTexts[5] = "Magic";
        hatTexts[6] = "Beach";
        hatTexts[7] = "Ribbon 2";
    }

    public Color GetBackgroundColor(int colorIndex)
    {
        return colors[colorIndex];
    }

    public string GetBackgroundHatText(int hatIndex)
    {
        return hatTexts[hatIndex];
    }

    [PunRPC]
    public void AskBodyColorUpdate(int enterOrder, int lastIndex, int wantBodyIndex, bool isRightButton, bool isFirstEntry) // 마스터 클라이언트에서 실행될 함수
    {
        wantBodyIndex = playerData.GetCapableBodyIndex(lastIndex, wantBodyIndex, isRightButton, isFirstEntry);
        
        UpdateBodyData(enterOrder, wantBodyIndex); // 플레이어의 몸색깔 데이터를 갱신해줌

        modelPVs[enterOrder].RPC("SetBodyColor", RpcTarget.AllBuffered, wantBodyIndex); // 플레이어의 몸색깔을 바꿔줌
        waitingViews[enterOrder].GetViewPV().RPC("UpdateBodyIndex", RpcTarget.AllBuffered, wantBodyIndex);

        Player askPlayer = players[enterOrder];
        waitingViews[enterOrder].GetViewPV().RPC("SetBackgroundColor", askPlayer, wantBodyIndex);
    }

    [PunRPC]
    public void AskHatUpdate(int enterOrder, int hatIndex) // 마스터 클라이언트에서 실행될 함수
    {
        UpdateHatData(enterOrder, hatIndex); // 플레이어의 모자 데이터를 갱신해줌

        modelPVs[enterOrder].RPC("SetHatOnPlayer", RpcTarget.AllBuffered, hatIndex); // 플레이어 모자를 바꿔줌

        Player askPlayer = players[enterOrder];
        waitingViews[enterOrder].GetViewPV().RPC("SetHatText", askPlayer, hatIndex);
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


    private int readyCount = 0;
    private const int maxReadyCount = 3;
    private bool isStartable = false;

    [PunRPC]
    public void SetReady(int enterOrder) // 마스터 클라이언트만 실행시켜줄 함수
    {
        if (isPlayerPresent[enterOrder])
        {
            if (isReady[enterOrder] == true) // 이미 레디한 상태라면
            {
                // 레디를 해제한다
                isReady[enterOrder] = false;
                waitingViews[enterOrder].GetViewPV().RPC("SetReadyColor", RpcTarget.AllBuffered, isReady[enterOrder]);
                --readyCount;
            }
            else // 레디를 안한 상태라면
            {
                // 레디한다
                isReady[enterOrder] = true;
                waitingViews[enterOrder].GetViewPV().RPC("SetReadyColor", RpcTarget.AllBuffered, isReady[enterOrder]);
                ++readyCount;
            }
        }

        CheckIfStartable(readyCount);
    }

    private void CheckIfStartable(int readyCount)
    {
        if (maxReadyCount <= readyCount)
        {
            isStartable = true;
        }
        else
        {
            isStartable = false;
        }

        waitingViews[1].GetViewPV().RPC("ActivateStartButton", RpcTarget.All, isStartable); // 방장의 StartButton 활성화
    }

    private void SetDefualtNames()
    {
        for (int i = 1; i < defaultNames.Length ;++i)
        {
            defaultNames[i] = $"Player {i}";
        }
    }

    private string GetDefualtName(int enterOrder)
    {
        return defaultNames[enterOrder];
    }

    #region Photon 콜백 함수들
    public override void OnJoinedRoom()
    {
        hatTypeCount = playerData.GetHatTypeCount();
        bodyColorCount = playerData.GetBodyColorCount();
        InitBackgrounds();
        roomName = PhotonNetwork.CurrentRoom.Name;
        roomNameText.text = roomName;
        amIOriginalMaster = PhotonNetwork.IsMasterClient;

        InitializeHashTable(); // 플레이어 커스텀 프로퍼티를 사용하기 위한 초기화 과정

        if (PhotonNetwork.IsMasterClient)
        {
            SetDefualtNames();

            isPlayerPresent[enterOrder] = true; // 들어옴 체크
            playerData.AddPlayerData(PhotonNetwork.LocalPlayer, enterOrder, GetDefualtName(enterOrder), enterOrder, 0); // Model(data) 업데이트
            //playerData.UpdateColorIndexing(enterOrder, true);
            // 플레이어 생성
            GameObject model = PhotonNetwork.Instantiate($"Prefabs/Lobby/WaitingRoomCanvas/RoomWait {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);

            PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // 모델체인저 뽑아옴
            modelChangers[enterOrder] = modelChanger; // 모델 체인저 저장해둠 --> 마스터가 다 컨트롤할라구
            modelPVs[enterOrder] = PhotonView.Get(model); // 모델체인저와 연동된 포톤뷰 저장해둠 -> 마스터가 다 컨트롤하라구

            waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder); // View 의 입장순서를 업데이트해줌

            players[enterOrder] = PhotonNetwork.LocalPlayer;
            AskBodyColorUpdate(enterOrder, enterOrder, enterOrder, true, true); // 색을 바꿔줌
        }

        
    }

    Hashtable[] playerProperties = new Hashtable[5];
    
    private void InitializeHashTable()
    {
        for (int i = 1; i < playerProperties.Length; ++i)
        {
            playerProperties[i] = new Hashtable();
        }
    }

    private string nameKey = "nickName";
    private string colorKey = "ColorIndex";
    private string hatKey = "HatIndex";
    public void LoadBoardGame()
    {
        SavePlayerProperties().Forget(); // 혹시 메인씬으로 로드하다가 고장날까봐 비동기로 처리
    }

    private async UniTaskVoid SavePlayerProperties()
    {
        for (int enterOrder = 1; enterOrder < players.Length; ++enterOrder)
        {
            Player player = players[enterOrder];
            if (player != null) // 혹시 모를 널 체크
            {
                string savedNickName = playerData.GetPlayerNickName(player);
                int savedColorIndex = playerData.GetPlayerBodyColorIndex(player);
                int savedHatIndex = playerData.GetPlayerHatIndex(player);

                playerProperties[enterOrder].Add(nameKey, savedNickName);
                playerProperties[enterOrder].Add(colorKey, savedColorIndex);
                playerProperties[enterOrder].Add(hatKey, savedHatIndex);
            }
        }

        MoveToBoardGame();
    }

    private void MoveToBoardGame()
    {
        PhotonNetwork.LoadLevel("BoardGame");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            enterOrder = GetEmptySlot(); // 빈자리를 찾아서, 입장순서를 정해줌
            isPlayerPresent[enterOrder] = true; // 존재함 체크
            playerData.AddPlayerData(newPlayer, enterOrder, GetDefualtName(enterOrder), enterOrder, 0); // Model 업데이트
            //playerData.UpdateColorIndexing(enterOrder, true);

            // 플레이어 생성
            GameObject model = PhotonNetwork.Instantiate($"Prefabs/Lobby/WaitingRoomCanvas/RoomWait {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);
            
            PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // 모델 체인저 뽑아옴
            modelChangers[enterOrder] = modelChanger; // 모델체인저 저장해둠 --> 마스터가 다 조종할라고
            modelPVs[enterOrder] = PhotonView.Get(model); // 모델체인저와 연동된 포톤뷰 저장해둠 -> 마스터가 다 컨트롤하라구


            // View 에 입장순서 할당해줌
            waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder);
            waitingViews[enterOrder].GetViewPV().TransferOwnership(newPlayer); // 소유권 양도해줌

            players[enterOrder] = newPlayer;
            AskBodyColorUpdate(enterOrder, enterOrder, enterOrder, true, true); // 색을 바꿔줌
            EnableRoomOpen();
        }
    }

    private void EnableRoomOpen()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        else
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) 
    {
        if (amIOriginalMaster && PhotonNetwork.IsMasterClient) // 마스터가해줘야 할 일들
        {
            int leftPlayerEnterOrder = playerData.GetPlayerEnterOrder(otherPlayer);
            Debug.Log($"플레이어가 나갈때 GetPlayerEnterOrder에서의 값 = {leftPlayerEnterOrder}");
            ResetBodyColor(leftPlayerEnterOrder);
            DestroyOtherPlayer(leftPlayerEnterOrder);
            isPlayerPresent[playerData.GetPlayerEnterOrder(otherPlayer)] = false; // 나감 표시
            playerData.UpdateColorIndexing(playerData.GetPlayerBodyColorIndex(otherPlayer), false);
            

            isReady[leftPlayerEnterOrder] = false;
            waitingViews[leftPlayerEnterOrder].GetViewPV().RPC("SetReadyColor", RpcTarget.AllBuffered, false);
            --readyCount;
            CheckIfStartable(readyCount);
            waitingViews[enterOrder].GetViewPV().RPC("ShowPlayerNickName", RpcTarget.AllBuffered, GetDefualtName(enterOrder));
            EnableRoomOpen();
            playerData.RemovePlayerData(otherPlayer); // MoDEL 업데이트
        }
    }
    
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // 마스터가 바뀌었다 -> 방이 폭파된다 -> 다 나가
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby Scene 3");
    }

    #endregion

    
    public void KickEveryeoneOut()
    {
        for (int enterOrder = isPlayerPresent.Length - 1; 0 < enterOrder; --enterOrder)
        {
            DestroyOtherPlayer(enterOrder);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void DestroyOtherPlayer(int enterOrder) // 마스터만 접근할 함수
    {
        if (modelChangers[enterOrder] != null)
        {
            GameObject hat = modelChangers[enterOrder].GetCurrentHat();

            if (hat != null)
            {
                modelChangers[enterOrder].RemoveCurrentHat();
            }
        }

        PhotonView photonView = modelPVs[enterOrder];
        if (photonView != null)
        {
            PhotonNetwork.Destroy(photonView); // 게임캐릭터 제거
        }
    }


    private int GetEmptySlot()
    {
        int emptyIndex = 9999;
        for (int i = 1; i < isPlayerPresent.Length; ++i)
        {
            if (isPlayerPresent[i] == false)
            {
                emptyIndex = i;
                return emptyIndex;
            }
        }

        Debug.Assert(emptyIndex <= isPlayerPresent.Length);
        return emptyIndex;
    }


    public void OnClick_LeaveRoom()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            for (int enterOrder = 1; enterOrder <= PhotonNetwork.CurrentRoom.PlayerCount; ++enterOrder)
            {
                ResetBodyColor(enterOrder);
            }
            KickEveryeoneOut();
        }
        
        LeaveRoom();
    }

    [PunRPC]
    public void SetPlayerNickName(int enterOrder, string newNickName)
    {
        Player updatePlayer = players[enterOrder];
        playerData.UpdateNickName(updatePlayer, newNickName);
        waitingViews[enterOrder].GetViewPV().RPC("ShowPlayerNickName", RpcTarget.AllBuffered, newNickName);
    }

}
