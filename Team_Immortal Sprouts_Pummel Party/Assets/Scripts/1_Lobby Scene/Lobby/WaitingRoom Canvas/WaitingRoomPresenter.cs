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
    [SerializeField] private LobbyPlayerData playerData;
    [SerializeField] private WaitingRoomView[] waitingViews;
    private PhotonView presenterPV;
    [SerializeField] private PositionData positionData;
    [SerializeField] private CustomData customData;

    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private int enterOrder = 1; // �׽�Ʈ ���� �־���
    [SerializeField] private GameObject[] playerModels;

    private string roomName;
    [SerializeField] private bool[] isReady = new bool[5];
    [SerializeField] private bool[] isPlayerPresent = new bool[5];

    public PhotonView GetPresenterPV()
    {
        if (presenterPV == null)
        {
            presenterPV = GetComponent<PhotonView>();
        }

        return presenterPV;
    }

    public void TellAllReady(int playerOrderNumber, Image readyBar)
    {
        // ����� �����ϰ�, ������ �����Ѵ�
        presenterPV.RPC("SetPlayerReady", RpcTarget.All, playerOrderNumber);

    }

    [PunRPC]
    private void SetPlayerReady(int playerOrderNumber)
    {
        if (isReady[playerOrderNumber] == true) // �̹� ������ ���¶��
        {
            // ���� �����Ѵ�
            isReady[playerOrderNumber] = false;
            waitingViews[playerOrderNumber].GetViewPV().RPC("SetReadyColor", RpcTarget.All, isReady[playerOrderNumber]);
        }
        else // ���� �Ǿ� ���� �ʴٸ�
        {
            // �����Ѵ�
            isReady[playerOrderNumber] = true;
            waitingViews[playerOrderNumber].GetViewPV().RPC("SetReadyColor", RpcTarget.All, isReady[playerOrderNumber]);
        }
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("BoardGame");
    }


    #region Photon �ݹ� �Լ���
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isPlayerPresent[enterOrder] = true;
            playerData.AddPlayerData(PhotonNetwork.LocalPlayer, 1, "Player 1", customData.GetBodyColorFromData(1), null);
            PhotonNetwork.Instantiate($"Prefabs/Lobby/WaitingRoomCanvas/RoomWait {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);
            waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder);
        }

        roomName = PhotonNetwork.CurrentRoom.Name;
        roomNameText.text = roomName;
    }




    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            enterOrder = GetEmptySlot();
            isPlayerPresent[enterOrder] = true;
            playerData.AddPlayerData(newPlayer, enterOrder, $"Player {enterOrder}", customData.GetBodyColorFromData(enterOrder), null);
            PhotonNetwork.Instantiate($"Prefabs/Lobby/WaitingRoomCanvas/RoomWait {enterOrder}", positionData._LobbyPositions[enterOrder].position, positionData._LobbyPositions[enterOrder].rotation);
            waitingViews[enterOrder].GetViewPV().RPC("SetEnterOrder", RpcTarget.AllBuffered, enterOrder);
        }

    }

    #endregion

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

    

    private async UniTaskVoid TurnOnAllPlayerModels()
    {
        await UniTask.Delay(2000);
        for (int i = 1; i < isPlayerPresent.Length; ++i)
        {
            if (isPlayerPresent[i] == true)
            {
                TurnOnLocalModel(i);
            }
        }
    }

    private void TurnOnLocalModel(int enterOrder)
    {
        playerModels[enterOrder].SetActive(true);
    }

    [PunRPC]
    public void TurnOnModel(int enterOrder)
    {
        playerModels[enterOrder].SetActive(true);
    }

    [PunRPC]
    public void TurnOffModel(int enterOrder)
    {
        playerModels[enterOrder].SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            for (int i = 1; i < isPlayerPresent.Length ;++i)
            {
                stream.SendNext(isPlayerPresent[i]);
            }
        }
        else if(stream.IsReading)
        {
            for (int i = 1; i < isPlayerPresent.Length ;++i)
            {
                isPlayerPresent[i] = (bool)stream.ReceiveNext();
            }
        }
    }
}
