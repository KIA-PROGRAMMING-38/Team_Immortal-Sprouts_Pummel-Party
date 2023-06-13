using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] private WaitingRoomCanvas waitingRoomCanvas;
    [SerializeField] private PlayerSlot playerSlot;
    [SerializeField] private TMP_Text nicknameInputField;
    [SerializeField] private Image bodyColor;
    [SerializeField] private Button bodyLeftButton;
    [SerializeField] private Button bodyRightButton;
    [SerializeField] private TMP_Text hatText;
    [SerializeField] private Button hatLeftButton;
    [SerializeField] private Button hatRightButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private CustomData customData;
    //[SerializeField] private int playerPositionIndex; 
    //[SerializeField] private CustomData customData;


    [SerializeField] private int bodyColorIndex = 0;// 테스트 위해 SerializeField 추가함
    [SerializeField] private int hatIndex = 0;// 테스트 위해 SerializeField 추가함
    [SerializeField] private int playerPositionIndex;// 테스트 위해 SerializeField 추가함

    private PhotonView customizeCanvasPhotonView;
    private const string hatPrefabPath = "Prefabs/Hats/";


    private void Start()
    {
        customizeCanvasPhotonView = PhotonView.Get(gameObject);
    }

    [PunRPC]
    public void SetCustomizeCanvasPlayerIndex(int playerPositionIndex)
    {
        this.playerPositionIndex = playerPositionIndex;
    }

    public void SetMasterCustomizeCanvasPlayerIndex(int playerPositionIndex)
    {
        this.playerPositionIndex = playerPositionIndex;
    }

    ///// <summary>
    ///// Customize Canvas의 플레이어 Index를 설정해준다
    ///// </summary>
    ///// <param name="playerPositionIndex"></param>
    //public void SetCustomizeCanvasPlayerIndex(int playerPositionIndex)
    //{
    //    this.playerPositionIndex = playerPositionIndex;
    //}


    [PunRPC]
    private void AskMasterClientToChangeBodyColor(int playerPositionIndex, int bodyIndex)
    {
        PlayerModelChanger askedPlayerModelChanger = waitingRoomCanvas.GetPlayerModelChanger(playerPositionIndex);
        PhotonView askedPhotonView = PhotonView.Get(askedPlayerModelChanger);
        askedPhotonView.RPC("SetBodyColor", RpcTarget.All, bodyIndex) ;
    }


    [PunRPC]
    private void AskMasterClientToChangeHat(int playerPositionIndex, int hatIndex)
    {
        PlayerModelChanger askedPlayerModelChanger = waitingRoomCanvas.GetPlayerModelChanger(playerPositionIndex);
        PhotonView askedPhotonView = PhotonView.Get(askedPlayerModelChanger);
        askedPhotonView.RPC("SetHatOnPlayer", RpcTarget.All, hatIndex);
    }

    [PunRPC]
    private void ChangeBodyColor(int playerPositionIndex, int bodyIndex)
    {
        PlayerModelChanger askedPlayerModelChanger = waitingRoomCanvas.GetPlayerModelChanger(playerPositionIndex);
        PhotonView askedPhotonView = PhotonView.Get(askedPlayerModelChanger);
        askedPhotonView.RPC("SetBodyColor", RpcTarget.All, bodyIndex);
    }

    [PunRPC]
    private void ChangeHat(int playerPositionIndex, int hatIndex)
    {
        PlayerModelChanger askedPlayerModelChanger = waitingRoomCanvas.GetPlayerModelChanger(playerPositionIndex);
        PhotonView askedPhotonView = PhotonView.Get(askedPlayerModelChanger);
        askedPhotonView.RPC("SetHatOnPlayer", RpcTarget.All, hatIndex);
    }


    private int SetButtonIndex(bool isRightButton, int index, int length) // 모자 7개(6), 몸색 8개(7)
    {
        if (isRightButton) // 오른쪽 버튼을 눌렀다면
        {
            // 인덱스가 증가해야한다
            ++index;
            if (length <= index)
            {
                index = 0;
            }
        }
        else // 왼쪽 버튼을 눌렀다면
        {
            // 인덱스가 감소해야한다
            --index;
            if (index < 0)
            {
                index = length- 1;
            }
        }

        return index;

    }

    #region OnClick 이벤트 함수

    public void OnClick_Body_LeftButton()
    {
        if (customizeCanvasPhotonView.IsMine)
        {
            bodyColorIndex = SetButtonIndex(false, bodyColorIndex, customData.bodyColors.Length);

            if (!PhotonNetwork.IsMasterClient) // 방장이 아니라면 방장에게 부탁한다
            {
                customizeCanvasPhotonView.RPC("AskMasterClientToChangeBodyColor", RpcTarget.MasterClient, playerPositionIndex, bodyColorIndex);
            }
            else // 방장이라면 
            {
                customizeCanvasPhotonView.RPC("ChangeBodyColor", RpcTarget.All, playerPositionIndex ,bodyColorIndex);
            }
        }
    }

    public void OnClick_Body_RightButton()
    {
        if (customizeCanvasPhotonView.IsMine)
        {
            bodyColorIndex = SetButtonIndex(true, bodyColorIndex, customData.bodyColors.Length);

            if (!PhotonNetwork.IsMasterClient) // 방장이 아니라면 방장에게 부탁한다
            {
                customizeCanvasPhotonView.RPC("AskMasterClientToChangeBodyColor", RpcTarget.MasterClient, playerPositionIndex, bodyColorIndex);
            }
            else // 방장이라면 
            {
                customizeCanvasPhotonView.RPC("ChangeBodyColor", RpcTarget.All, playerPositionIndex, bodyColorIndex);
            }
        }
    }

    public void OnClick_Hat_LeftButton()
    {
        hatIndex = SetButtonIndex(false, hatIndex, customData.hats.Length);

        if (!PhotonNetwork.IsMasterClient) // 방장이 아니라면 방장에게 부탁한다
        {
            customizeCanvasPhotonView.RPC("AskMasterClientToChangeHat", RpcTarget.MasterClient, playerPositionIndex, hatIndex);
        }
        else // 방장이라면 
        {
            customizeCanvasPhotonView.RPC("ChangeHat", RpcTarget.All, playerPositionIndex , hatIndex);
        }
    }

    public void OnClick_Hat_RightButton()
    {
        hatIndex = SetButtonIndex(true, hatIndex, customData.hats.Length);

        if (!PhotonNetwork.IsMasterClient) // 방장이 아니라면 방장에게 부탁한다
        {
            customizeCanvasPhotonView.RPC("AskMasterClientToChangeHat", RpcTarget.MasterClient, playerPositionIndex, hatIndex);
        }
        else // 방장이라면 
        {
            customizeCanvasPhotonView.RPC("ChangeHat", RpcTarget.All, playerPositionIndex, hatIndex);
        }
    }

    public void OnClick_ConfirmButton()
    {
        playerSlot.ActivateCustomizeCanvas(false);
        SetPlayerNickname(nicknameInputField.text);
    }

    #endregion



    private void SetPlayerNickname(string inputPlayerName)
    {
        playerSlot.GetSelectCanvas().SetPlayerNickName(inputPlayerName);
    }
}
