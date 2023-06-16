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

    [SerializeField] private int enterOrder = 1; 
    [SerializeField] private bool[] isReady = new bool[MAX_INDEX];
    [SerializeField] private bool[] isPlayerPresent = new bool[MAX_INDEX];

    [SerializeField] private GameObject[] playerModels;
    [SerializeField] private PlayerModelChanger[] modelChangers = new PlayerModelChanger[MAX_INDEX];
    [SerializeField] private PhotonView[] modelPVs = new PhotonView[MAX_INDEX];

    [SerializeField] private Player[] players = new Player[MAX_INDEX];
    private const int MAX_INDEX = 5; // 0��° �ε����� �����ϱ� ����

    public int hatTypeCount { get; private set; }
    public int bodyColorCount { get; private set; }

    private bool amIOriginalMaster = false; // ó�� �濡 �������� ������ �������� �ƴ����� �����ϴ� ����

    private string[] defaultNames = new string[MAX_INDEX];


    /// <summary>
    /// �־��� ��������� �ش��ϴ� �÷��̾��� ������ �ʱ�ȭ ���ִ� �Լ�
    /// </summary>
    /// <param name="enterOrder"></param>
    public void ResetBodyColor(int enterOrder) // �����Ͱ� ����
    {
        if (modelPVs[enterOrder] != null)
        {
            modelPVs[enterOrder].RPC("SetBodyColor", RpcTarget.AllBuffered, enterOrder); // �ٵ� Į�� ����
        }
    }

    /// <summary>
    /// �־��� �ε����� ���� UI���� �� ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="colorIndex"></param>
    /// <returns></returns>
    public Color GetBackgroundColor(int colorIndex)
    {
        return playerData.GetBackgroundColorData(colorIndex);
    }

    public string GetBackgroundHatText(int hatIndex)
    {
        return playerData.GetBackgroundHatTextData(hatIndex);
    }


    /// <summary>
    /// �����Ϳ��� �÷��̾� ������ �ٲ�޶�� ��û�ϴ� �Լ�
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="lastIndex"></param>
    /// <param name="wantBodyIndex"></param>
    /// <param name="isRightButton"></param>
    /// <param name="isFirstEntry"></param>
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


    /// <summary>
    /// �����Ϳ��� ���ڸ� �ٲ�޶�� ��û�ϴ� �Լ�
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="hatIndex"></param>
    [PunRPC]
    public void AskHatUpdate(int enterOrder, int hatIndex) // ������ Ŭ���̾�Ʈ���� ����� �Լ�
    {
        UpdateHatData(enterOrder, hatIndex); // �÷��̾��� ���� �����͸� ��������

        modelPVs[enterOrder].RPC("SetHatOnPlayer", RpcTarget.AllBuffered, hatIndex); // �÷��̾� ���ڸ� �ٲ���

        Player askPlayer = players[enterOrder];
        waitingViews[enterOrder].GetViewPV().RPC("SetHatText", askPlayer, hatIndex);
    }


    /// <summary>
    /// �����Ϳ� �÷��̾� ���� �ε��� ������ ������Ʈ �ϴ� �Լ�
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="bodyIndex"></param>
    private void UpdateBodyData(int enterOrder, int bodyIndex) // �����͸� �����ϱ� ���� ������ Ŭ���̾�Ʈ�� ������ �Լ�
    {
        Player updatePlayer = players[enterOrder];

        playerData.UpdateBodyIndex(updatePlayer, bodyIndex);

    }

    /// <summary>
    /// �����Ϳ� �÷��̾� ���� �ε��� ������ ������Ʈ �ϴ� �Լ�
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="hatIndex"></param>
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
    private const int MAX_READY_COUNT = 3;
    private bool isStartable = false;

    /// <summary>
    /// �����Ͱ� �־��� ��������� �÷��̾ �����Ű�� �Լ�
    /// </summary>
    /// <param name="enterOrder"></param>
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

    /// <summary>
    /// ���� ������ �������� �Ǵ� ��, ���۹�ư�� Ȱ��ȭ �ϴ� �Լ�
    /// </summary>
    /// <param name="readyCount"></param>
    private void CheckIfStartable(int readyCount)
    {
        if (MAX_READY_COUNT <= readyCount)
        {
            isStartable = true;
        }
        else
        {
            isStartable = false;
        }

        waitingViews[1].GetViewPV().RPC("ActivateStartButton", RpcTarget.All, isStartable); // ������ StartButton Ȱ��ȭ
    }

    /// <summary>
    /// �÷��̾� �⺻�̸��� �ʱ�ȭ �ϴ� �Լ� 
    /// </summary>
    private void SetDefualtNames()
    {
        for (int i = 1; i < defaultNames.Length ;++i)
        {
            defaultNames[i] = $"Player {i}";
        }
    }

    /// <summary>
    /// �÷��̾� �⺻ �̸��� ��ȯ�ϴ� �Լ� 
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <returns></returns>
    private string GetDefualtName(int enterOrder)
    {
        return defaultNames[enterOrder];
    }


    private string modelPath = "Prefabs/Lobby/WaitingRoomCanvas/RoomWait";

    #region Photon �ݹ� �Լ���
    public override void OnJoinedRoom()
    {
        hatTypeCount = playerData.GetHatTypeCount();
        bodyColorCount = playerData.GetBodyColorCount();
        roomName = PhotonNetwork.CurrentRoom.Name;
        roomNameText.text = roomName;
        amIOriginalMaster = PhotonNetwork.IsMasterClient;

        InitializeHashTable(); // �÷��̾� Ŀ���� ������Ƽ�� ����ϱ� ���� �ʱ�ȭ ����

        if (PhotonNetwork.IsMasterClient)
        {
            SetDefualtNames();

            isPlayerPresent[enterOrder] = true; // ���� üũ
            playerData.AddPlayerData(PhotonNetwork.LocalPlayer, enterOrder, GetDefualtName(enterOrder), enterOrder, 0); // Model(data) ������Ʈ
            
            // �÷��̾� ����
            GameObject model = PhotonNetwork.Instantiate($"{modelPath} {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);

            PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // ��ü���� �̾ƿ�
            modelChangers[enterOrder] = modelChanger; // �� ü���� �����ص� --> �����Ͱ� �� ��Ʈ���Ҷ�
            modelPVs[enterOrder] = PhotonView.Get(model); // ��ü������ ������ ����� �����ص� -> �����Ͱ� �� ��Ʈ���϶�

            waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder); // View �� ��������� ������Ʈ����

            players[enterOrder] = PhotonNetwork.LocalPlayer;
            AskBodyColorUpdate(enterOrder, enterOrder, enterOrder, true, true); // ���� �ٲ���
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            enterOrder = GetEmptySlot(); // ���ڸ��� ã�Ƽ�, ��������� ������
            isPlayerPresent[enterOrder] = true; // ������ üũ
            playerData.AddPlayerData(newPlayer, enterOrder, GetDefualtName(enterOrder), enterOrder, 0); // Model ������Ʈ

            // �÷��̾� ����
            GameObject model = PhotonNetwork.Instantiate($"{modelPath} {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);
            
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


    public override void OnPlayerLeftRoom(Player otherPlayer) 
    {
        if (amIOriginalMaster && PhotonNetwork.IsMasterClient) // �����Ͱ������ �� �ϵ�
        {
            int leftPlayerEnterOrder = playerData.GetPlayerEnterOrder(otherPlayer);
            ResetBodyColor(leftPlayerEnterOrder);
            DestroyOtherPlayer(leftPlayerEnterOrder);
            isPlayerPresent[playerData.GetPlayerEnterOrder(otherPlayer)] = false; // ���� ǥ��
            playerData.UpdateColorIndexing(playerData.GetPlayerBodyColorIndex(otherPlayer), false);

            // �÷��̾ �������� �⺻������ �����ִ� �κ�
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

    Hashtable[] playerProperties = new Hashtable[5];

    private void InitializeHashTable()
    {
        for (int i = 1; i < playerProperties.Length; ++i)
        {
            playerProperties[i] = new Hashtable();
        }
    }

    private string nameKey = "NickName";
    private string colorKey = "ColorIndex";
    private string hatKey = "HatIndex";
    private string hpKey = "HP";
    private string eggCountKey = "EggCount";
    private string positionKey = "Position";
    private string enterOrderKey = "EnterOrder";

    private int playerMaxHP = 30;
    private Vector3 defualtPosition = Vector3.zero;
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
                int playerEnterOrder = playerData.GetPlayerEnterOrder(player);

                playerProperties[enterOrder].Add(nameKey, savedNickName);
                playerProperties[enterOrder].Add(colorKey, savedColorIndex);
                playerProperties[enterOrder].Add(hatKey, savedHatIndex);
                playerProperties[enterOrder].Add(enterOrderKey, playerEnterOrder);
                playerProperties[enterOrder].Add(hpKey, playerMaxHP);
                playerProperties[enterOrder].Add(eggCountKey, 0);
                playerProperties[enterOrder].Add(positionKey, defualtPosition);
            }
        }

        MoveToBoardGame();
    }

    private void MoveToBoardGame()
    {
        PhotonNetwork.LoadLevel("BoardGame");
    }

    private void KickEveryoneOut()
    {
        // ������� ������ MasterClient�� �Űܰ��� ������, �̷��� ó����
        for (int enterOrder = isPlayerPresent.Length - 1; 0 < enterOrder; --enterOrder)
        {
            DestroyOtherPlayer(enterOrder);
        }
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void DestroyOtherPlayer(int enterOrder) // �����͸� ������ �Լ�
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


    public void OnClick_LeaveRoom() // Leave ��ư�� �� �Լ� => Public �̿��� ��
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
    /// �÷��̾��� �г����� �����Ϳ� ������Ʈ�ϰ�, �ٸ� �÷��̾�鿡�� �����ִ� �Լ�
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="newNickName"></param>
    [PunRPC]
    public void SetPlayerNickName(int enterOrder, string newNickName)
    {
        Player updatePlayer = players[enterOrder];
        playerData.UpdateNickName(updatePlayer, newNickName);
        waitingViews[enterOrder].GetViewPV().RPC("ShowPlayerNickName", RpcTarget.AllBuffered, newNickName);
    }

}
