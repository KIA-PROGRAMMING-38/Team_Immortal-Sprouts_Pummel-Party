using Cysharp.Threading.Tasks;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] PlayerSlot playerSlot;
    [SerializeField] TMP_Text playerNickName;
    [SerializeField] Button customizeButton;
    [SerializeField] Button readyButton;
    [SerializeField] PhotonView masterPlayerSlotPhotonView;

    private PhotonView playerSlotPhotonView;

    [SerializeField] private bool isReady = false; // 테스트 위해 SereializeField 추가함

    /// <summary>
    /// 플레이어의 닉네임을 설정해준다
    /// </summary>
    /// <param name="inputNickName"></param>
    public void SetPlayerNickName(string inputNickName)
    {
        playerNickName.text = inputNickName;
    }

    /// <summary>
    /// 각 플레이어의 ready 상태를 false로 리셋한다
    /// </summary>
    public void ResetReadyStatus()
    {
        isReady = false;
    }

    /// <summary>
    /// 방장의 startButton의 interactable 을 false로 리셋한다
    /// </summary>
    public void ResetStartButton()
    {
        readyButton.interactable = false;
    }

    /// <summary>
    /// readyCount의 조건에 따라서 startButton을 활성화시킨다
    /// </summary>
    /// <param name="readyCount"></param>
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

    /// <summary>
    /// 각 플레이어의 커스터마이즈와 레디 버튼을 활성화시킨다
    /// </summary>
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
        playerSlot.ActivateCustomizeCanvas(true);
    }

    #endregion


}
