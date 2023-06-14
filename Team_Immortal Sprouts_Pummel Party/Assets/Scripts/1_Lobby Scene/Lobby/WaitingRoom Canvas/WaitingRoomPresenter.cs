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

    public int hatTypeCount { get; private set; }
    public int bodyColorCount { get; private set; }



    [PunRPC]
    public void AskBodyColorUpdate(int enterOrder, int lastIndex, int wantBodyIndex, bool isRightButton) // ������ Ŭ���̾�Ʈ���� ����� �Լ�
    {
        wantBodyIndex = playerData.GetCapableBodyIndex(lastIndex, wantBodyIndex, isRightButton);
            
        UpdateBodyData(enterOrder, wantBodyIndex); // �÷��̾��� ������ �����͸� ��������

        modelPVs[enterOrder].RPC("SetBodyColor", RpcTarget.AllBuffered, wantBodyIndex); // �÷��̾��� �������� �ٲ���
        waitingViews[enterOrder].GetViewPV().RPC("UpdateBodyIndex", RpcTarget.AllBuffered, wantBodyIndex);
    }

    [PunRPC]
    public void AskHatUpdate(int enterOrder, int hatIndex) // ������ Ŭ���̾�Ʈ���� ����� �Լ�
    {
        UpdateHatData(enterOrder, hatIndex); // �÷��̾��� ���� �����͸� ��������

        modelPVs[enterOrder].RPC("SetHatOnPlayer", RpcTarget.AllBuffered, hatIndex); // �÷��̾� ���ڸ� �ٲ���
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

    public PhotonView GetMasterPV()
    {
        return waitingViews[1].GetViewPV();
    }

    [PunRPC]
    public void SetReady(int enterOrder)
    {
        if (isReady[enterOrder] == true) // �̹� ������ ���¶��
        {
            // ���� �����Ѵ�
            isReady[enterOrder] = false;
            waitingViews[enterOrder].GetViewPV().RPC("SetReadyColor", RpcTarget.All, isReady[enterOrder]);
        }
        else
        {
            // �����Ѵ�
            isReady[enterOrder] = true;
            waitingViews[enterOrder].GetViewPV().RPC("SetReadyColor", RpcTarget.All, isReady[enterOrder]);
        }
    }

    #region Photon �ݹ� �Լ���
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isPlayerPresent[enterOrder] = true; // ���� üũ
            playerData.AddPlayerData(PhotonNetwork.LocalPlayer, enterOrder, $"Player {enterOrder}", enterOrder, 0); // Model(data) ������Ʈ
            playerData.UpdateColorIndexing(enterOrder);
            // �÷��̾� ����
            GameObject model = PhotonNetwork.Instantiate($"Prefabs/Lobby/WaitingRoomCanvas/RoomWait {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);

            PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // ��ü���� �̾ƿ�
            modelChangers[enterOrder] = modelChanger; // �� ü���� �����ص� --> �����Ͱ� �� ��Ʈ���Ҷ�
            modelPVs[enterOrder] = PhotonView.Get(model); // ��ü������ ������ ����� �����ص� -> �����Ͱ� �� ��Ʈ���϶�

            waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder); // View �� ��������� ������Ʈ����

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
            enterOrder = GetEmptySlot(); // ���ڸ��� ã�Ƽ�, ��������� ������
            isPlayerPresent[enterOrder] = true; // ������ üũ
            playerData.AddPlayerData(newPlayer, enterOrder, $"Player {enterOrder}", enterOrder, 0); // Model ������Ʈ
            playerData.UpdateColorIndexing(enterOrder);

            // �÷��̾� ����
            GameObject model = PhotonNetwork.Instantiate($"Prefabs/Lobby/WaitingRoomCanvas/RoomWait {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);

            PlayerModelChanger modelChanger = model.GetComponent<PlayerModelChanger>(); // �� ü���� �̾ƿ�
            modelChangers[enterOrder] = modelChanger; // ��ü���� �����ص� --> �����Ͱ� �� �����Ҷ��
            modelPVs[enterOrder] = PhotonView.Get(model); // ��ü������ ������ ����� �����ص� -> �����Ͱ� �� ��Ʈ���϶�


            // View �� ������� �Ҵ�����
            waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder);
            waitingViews[enterOrder].GetViewPV().TransferOwnership(newPlayer); // ������ �絵����

            players[enterOrder] = newPlayer;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isPlayerPresent[playerData.GetPlayerEnterOrder(otherPlayer)] = false; // ���� ǥ��
            playerData.RemovePlayerData(otherPlayer); // MoDEL ������Ʈ
            //PhotonNetwork.Destroy(modelPVs[playerData.GetPlayerEnterOrder(otherPlayer)]); // ����ĳ���� ����
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // �����Ͱ� �ٲ���� -> ���� ���ĵȴ� -> �� ����
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



    
    public void DestroyOtherPlayer(int enterOrder) // �����͸� ������ �Լ�
    {
        PhotonView photonView= modelPVs[enterOrder];
        if (photonView != null)
        {
            PhotonNetwork.Destroy(photonView); // ����ĳ���� ����
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
