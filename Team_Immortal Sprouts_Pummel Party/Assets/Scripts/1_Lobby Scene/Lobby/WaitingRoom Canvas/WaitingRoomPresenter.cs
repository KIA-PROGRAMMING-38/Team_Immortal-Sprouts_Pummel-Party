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
    [SerializeField] private LobbyPlayerData playerData; // ���� ��

    [SerializeField] private WaitingRoomView[] waitingViews;

    [SerializeField] private PositionData positionData;
    [SerializeField] private CustomData customData;


    [SerializeField] private int enterOrder = 1; // �׽�Ʈ ���� �־���
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

    public void ResetBodyColor(int enterOrder) // �����Ͱ� ����
    {
        if (modelPVs[enterOrder] != null)
        {
            modelPVs[enterOrder].RPC("SetBodyColor", RpcTarget.AllBuffered, enterOrder); // �ٵ� Į�� ����
        }
    }

    private void InitBackgrounds()
    {
        colors = new Color[bodyColorCount];
        colors[0] = Color.black;
        colors[1] = Color.white;
        colors[2] = new Color(255f / 255f, 160f / 255f, 219f / 255f); // ��ũ
        colors[3] = Color.green;
        colors[4] = Color.red;
        colors[5] = Color.blue;
        colors[6] = new Color(210f / 255f, 122f / 255f, 79f / 255f); // ������
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
    public void AskBodyColorUpdate(int enterOrder, int lastIndex, int wantBodyIndex, bool isRightButton, bool isFirstEntry) // ������ Ŭ���̾�Ʈ���� ����� �Լ�
    {
        wantBodyIndex = playerData.GetCapableBodyIndex(lastIndex, wantBodyIndex, isRightButton, isFirstEntry);
        
        UpdateBodyData(enterOrder, wantBodyIndex); // �÷��̾��� ������ �����͸� ��������

        modelPVs[enterOrder].RPC("SetBodyColor", RpcTarget.AllBuffered, wantBodyIndex); // �÷��̾��� �������� �ٲ���
        waitingViews[enterOrder].GetViewPV().RPC("UpdateBodyIndex", RpcTarget.AllBuffered, wantBodyIndex);

        Player askPlayer = players[enterOrder];
        waitingViews[enterOrder].GetViewPV().RPC("SetBackgroundColor", askPlayer, wantBodyIndex);
    }

    [PunRPC]
    public void AskHatUpdate(int enterOrder, int hatIndex) // ������ Ŭ���̾�Ʈ���� ����� �Լ�
    {
        UpdateHatData(enterOrder, hatIndex); // �÷��̾��� ���� �����͸� ��������

        modelPVs[enterOrder].RPC("SetHatOnPlayer", RpcTarget.AllBuffered, hatIndex); // �÷��̾� ���ڸ� �ٲ���

        Player askPlayer = players[enterOrder];
        waitingViews[enterOrder].GetViewPV().RPC("SetHatText", askPlayer, hatIndex);
    }

    private void UpdateBodyData(int enterOrder, int bodyIndex) // �����͸� �����ϱ� ���� ������ Ŭ���̾�Ʈ�� ������ �Լ�
    {
        Player updatePlayer = players[enterOrder];

        playerData.UpdateBodyIndex(updatePlayer, bodyIndex);

    }

    private void UpdateHatData(int enterOrder, int hatIndex) // �����͸� �����ϱ� ���� ������ Ŭ���̾�Ʈ�� ������ �Լ�
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
    public void SetReady(int enterOrder) // ������ Ŭ���̾�Ʈ�� ��������� �Լ�
    {
        if (isPlayerPresent[enterOrder])
        {
            if (isReady[enterOrder] == true) // �̹� ������ ���¶��
            {
                // ���� �����Ѵ�
                isReady[enterOrder] = false;
                waitingViews[enterOrder].GetViewPV().RPC("SetReadyColor", RpcTarget.AllBuffered, isReady[enterOrder]);
                --readyCount;
            }
            else // ���� ���� ���¶��
            {
                // �����Ѵ�
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

        waitingViews[1].GetViewPV().RPC("ActivateStartButton", RpcTarget.All, isStartable); // ������ StartButton Ȱ��ȭ
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

    #region Photon �ݹ� �Լ���
    public override void OnJoinedRoom()
    {
        hatTypeCount = playerData.GetHatTypeCount();
        bodyColorCount = playerData.GetBodyColorCount();
        InitBackgrounds();
        roomName = PhotonNetwork.CurrentRoom.Name;
        roomNameText.text = roomName;
        amIOriginalMaster = PhotonNetwork.IsMasterClient;

        InitializeHashTable(); // �÷��̾� Ŀ���� ������Ƽ�� ����ϱ� ���� �ʱ�ȭ ����

        if (PhotonNetwork.IsMasterClient)
        {
            SetDefualtNames();

            isPlayerPresent[enterOrder] = true; // ���� üũ
            playerData.AddPlayerData(PhotonNetwork.LocalPlayer, enterOrder, GetDefualtName(enterOrder), enterOrder, 0); // Model(data) ������Ʈ
            //playerData.UpdateColorIndexing(enterOrder, true);
            // �÷��̾� ����
            GameObject model = PhotonNetwork.Instantiate($"Prefabs/Lobby/WaitingRoomCanvas/RoomWait {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);

            PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // ��ü���� �̾ƿ�
            modelChangers[enterOrder] = modelChanger; // �� ü���� �����ص� --> �����Ͱ� �� ��Ʈ���Ҷ�
            modelPVs[enterOrder] = PhotonView.Get(model); // ��ü������ ������ ����� �����ص� -> �����Ͱ� �� ��Ʈ���϶�

            waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder); // View �� ��������� ������Ʈ����

            players[enterOrder] = PhotonNetwork.LocalPlayer;
            AskBodyColorUpdate(enterOrder, enterOrder, enterOrder, true, true); // ���� �ٲ���
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
        SavePlayerProperties().Forget(); // Ȥ�� ���ξ����� �ε��ϴٰ� ���峯��� �񵿱�� ó��
    }

    private async UniTaskVoid SavePlayerProperties()
    {
        for (int enterOrder = 1; enterOrder < players.Length; ++enterOrder)
        {
            Player player = players[enterOrder];
            if (player != null) // Ȥ�� �� �� üũ
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
            enterOrder = GetEmptySlot(); // ���ڸ��� ã�Ƽ�, ��������� ������
            isPlayerPresent[enterOrder] = true; // ������ üũ
            playerData.AddPlayerData(newPlayer, enterOrder, GetDefualtName(enterOrder), enterOrder, 0); // Model ������Ʈ
            //playerData.UpdateColorIndexing(enterOrder, true);

            // �÷��̾� ����
            GameObject model = PhotonNetwork.Instantiate($"Prefabs/Lobby/WaitingRoomCanvas/RoomWait {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);
            
            PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // �� ü���� �̾ƿ�
            modelChangers[enterOrder] = modelChanger; // ��ü���� �����ص� --> �����Ͱ� �� �����Ҷ��
            modelPVs[enterOrder] = PhotonView.Get(model); // ��ü������ ������ ����� �����ص� -> �����Ͱ� �� ��Ʈ���϶�


            // View �� ������� �Ҵ�����
            waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder);
            waitingViews[enterOrder].GetViewPV().TransferOwnership(newPlayer); // ������ �絵����

            players[enterOrder] = newPlayer;
            AskBodyColorUpdate(enterOrder, enterOrder, enterOrder, true, true); // ���� �ٲ���
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
        if (amIOriginalMaster && PhotonNetwork.IsMasterClient) // �����Ͱ������ �� �ϵ�
        {
            int leftPlayerEnterOrder = playerData.GetPlayerEnterOrder(otherPlayer);
            Debug.Log($"�÷��̾ ������ GetPlayerEnterOrder������ �� = {leftPlayerEnterOrder}");
            ResetBodyColor(leftPlayerEnterOrder);
            DestroyOtherPlayer(leftPlayerEnterOrder);
            isPlayerPresent[playerData.GetPlayerEnterOrder(otherPlayer)] = false; // ���� ǥ��
            playerData.UpdateColorIndexing(playerData.GetPlayerBodyColorIndex(otherPlayer), false);
            

            isReady[leftPlayerEnterOrder] = false;
            waitingViews[leftPlayerEnterOrder].GetViewPV().RPC("SetReadyColor", RpcTarget.AllBuffered, false);
            --readyCount;
            CheckIfStartable(readyCount);
            waitingViews[enterOrder].GetViewPV().RPC("ShowPlayerNickName", RpcTarget.AllBuffered, GetDefualtName(enterOrder));
            EnableRoomOpen();
            playerData.RemovePlayerData(otherPlayer); // MoDEL ������Ʈ
        }
    }
    
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // �����Ͱ� �ٲ���� -> ���� ���ĵȴ� -> �� ����
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

    public void DestroyOtherPlayer(int enterOrder) // �����͸� ������ �Լ�
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
            PhotonNetwork.Destroy(photonView); // ����ĳ���� ����
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
