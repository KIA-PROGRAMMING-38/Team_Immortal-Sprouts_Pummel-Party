using Cysharp.Threading.Tasks;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] PlayerSlot playerSlot;
    [SerializeField] Button customizeButton;
    [SerializeField] Button readyButton;
    [SerializeField] PhotonView masterPlayerSlotPhotonView;

    private PhotonView playerSlotPhotonView;

    [SerializeField] private bool isReady = false; // 테스트 위해 SereializeField 추가함

    public void ResetReadyStatus()
    {
        isReady = false;
    }

    public void ResetStartButton()
    {
        readyButton.interactable = false;
    }

    public void CheckAndEnableStartButton(int readyCount)
    {
        if (readyCount == 3)
        {
            readyButton.interactable = true;
        }
        else
        {
            readyButton.interactable = false;
        }
    }


    private void Start()
    {
        playerSlotPhotonView = playerSlot.GetPhotonView();
        
    }

    public void EnableButtons()
    {
        customizeButton.interactable = true;
        readyButton.interactable = true;
    }


    #region OnClick 이벤트 함수

    public void OnClick_StartButton()
    {
        PhotonNetwork.LoadLevel("BoardGame");
    }

    public void OnClick_ReadyButton()
    {
        if (!PhotonNetwork.IsMasterClient && playerSlotPhotonView.IsMine)
        {
            if (isReady)
            {
                masterPlayerSlotPhotonView.RPC("UnSetReady", RpcTarget.MasterClient);
                playerSlotPhotonView.RPC("SetNotReadyColor", RpcTarget.AllBuffered);
                isReady = false;
            }
            else 
            {
                masterPlayerSlotPhotonView.RPC("SetReady", RpcTarget.MasterClient);
                playerSlotPhotonView.RPC("SetReadyColor", RpcTarget.AllBuffered);
                isReady = true;
            }
        }
        
    }

    public void OnClick_CustomizeButton()
    {

    }

    #endregion


}
