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


    [SerializeField] private int bodyColorIndex = 0;// �׽�Ʈ ���� SerializeField �߰���
    [SerializeField] private int hatIndex = 0;// �׽�Ʈ ���� SerializeField �߰���
    [SerializeField] private int playerPositionIndex;// �׽�Ʈ ���� SerializeField �߰���

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
    ///// Customize Canvas�� �÷��̾� Index�� �������ش�
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


    private int SetButtonIndex(bool isRightButton, int index, int length) // ���� 7��(6), ���� 8��(7)
    {
        if (isRightButton) // ������ ��ư�� �����ٸ�
        {
            // �ε����� �����ؾ��Ѵ�
            ++index;
            if (length <= index)
            {
                index = 0;
            }
        }
        else // ���� ��ư�� �����ٸ�
        {
            // �ε����� �����ؾ��Ѵ�
            --index;
            if (index < 0)
            {
                index = length- 1;
            }
        }

        return index;

    }

    #region OnClick �̺�Ʈ �Լ�

    public void OnClick_Body_LeftButton()
    {
        if (customizeCanvasPhotonView.IsMine)
        {
            bodyColorIndex = SetButtonIndex(false, bodyColorIndex, customData.bodyColors.Length);

            if (!PhotonNetwork.IsMasterClient) // ������ �ƴ϶�� ���忡�� ��Ź�Ѵ�
            {
                customizeCanvasPhotonView.RPC("AskMasterClientToChangeBodyColor", RpcTarget.MasterClient, playerPositionIndex, bodyColorIndex);
            }
            else // �����̶�� 
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

            if (!PhotonNetwork.IsMasterClient) // ������ �ƴ϶�� ���忡�� ��Ź�Ѵ�
            {
                customizeCanvasPhotonView.RPC("AskMasterClientToChangeBodyColor", RpcTarget.MasterClient, playerPositionIndex, bodyColorIndex);
            }
            else // �����̶�� 
            {
                customizeCanvasPhotonView.RPC("ChangeBodyColor", RpcTarget.All, playerPositionIndex, bodyColorIndex);
            }
        }
    }

    public void OnClick_Hat_LeftButton()
    {
        hatIndex = SetButtonIndex(false, hatIndex, customData.hats.Length);

        if (!PhotonNetwork.IsMasterClient) // ������ �ƴ϶�� ���忡�� ��Ź�Ѵ�
        {
            customizeCanvasPhotonView.RPC("AskMasterClientToChangeHat", RpcTarget.MasterClient, playerPositionIndex, hatIndex);
        }
        else // �����̶�� 
        {
            customizeCanvasPhotonView.RPC("ChangeHat", RpcTarget.All, playerPositionIndex , hatIndex);
        }
    }

    public void OnClick_Hat_RightButton()
    {
        hatIndex = SetButtonIndex(true, hatIndex, customData.hats.Length);

        if (!PhotonNetwork.IsMasterClient) // ������ �ƴ϶�� ���忡�� ��Ź�Ѵ�
        {
            customizeCanvasPhotonView.RPC("AskMasterClientToChangeHat", RpcTarget.MasterClient, playerPositionIndex, hatIndex);
        }
        else // �����̶�� 
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
