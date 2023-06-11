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

    [SerializeField] private bool isReady = false; // �׽�Ʈ ���� SereializeField �߰���

    /// <summary>
    /// �÷��̾��� �г����� �������ش�
    /// </summary>
    /// <param name="inputNickName"></param>
    public void SetPlayerNickName(string inputNickName)
    {
        playerNickName.text = inputNickName;
    }

    /// <summary>
    /// �� �÷��̾��� ready ���¸� false�� �����Ѵ�
    /// </summary>
    public void ResetReadyStatus()
    {
        isReady = false;
    }

    /// <summary>
    /// ������ startButton�� interactable �� false�� �����Ѵ�
    /// </summary>
    public void ResetStartButton()
    {
        readyButton.interactable = false;
    }

    /// <summary>
    /// readyCount�� ���ǿ� ���� startButton�� Ȱ��ȭ��Ų��
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
    /// �� �÷��̾��� Ŀ���͸������ ���� ��ư�� Ȱ��ȭ��Ų��
    /// </summary>
    public void EnableButtons()
    {
        customizeButton.interactable = true;
        readyButton.interactable = true;
    }


    #region OnClick �̺�Ʈ �Լ�

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
