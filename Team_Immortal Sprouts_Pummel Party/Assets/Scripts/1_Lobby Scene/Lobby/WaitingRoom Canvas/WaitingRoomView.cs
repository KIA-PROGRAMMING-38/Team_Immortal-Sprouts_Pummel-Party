using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class WaitingRoomView : MonoBehaviourPunCallbacks
{
    [SerializeField] private WaitingRoomPresenter presenter;


    [SerializeField] private Image readyBar;
    [SerializeField] private Canvas selectCanvas;
    [SerializeField] private Canvas customizeCanvas;


    PhotonView viewPV;
    [SerializeField] private int enterOrder;

    private Color readyColor = Color.green;
    private Color notReadyColor = Color.red;
    [SerializeField] private int wantBodyIndex = 0;
    [SerializeField] private int hatIndex = 0;

    #region Public ÇÔ¼öµé
    public PhotonView GetViewPV()
    {
        if (viewPV == null)
        {
            viewPV = GetComponent<PhotonView>();
        }

        return viewPV;
    }

    [PunRPC]
    public void UpdateBodyIndex(int bodyIndex)
    {
        wantBodyIndex = bodyIndex;
    }

    [PunRPC]
    public void SetEnterOrder(int enterOrder)
    {
        this.enterOrder = enterOrder;
        this.wantBodyIndex = enterOrder;
    }

    [PunRPC]
    public void SetReadyColor(bool isReady)
    {
        if (isReady == true)
        {
            readyBar.color = notReadyColor;
        }
        else
        {
            readyBar.color = readyColor;
        }
    }

    #endregion

    #region OnClick Event ÇÔ¼öµé


    public void OnClick_CustomizeButton()
    {
        if (GetViewPV().IsMine)
        {
            selectCanvas.enabled = false;
            customizeCanvas.enabled = true;
        }
    }
    public void OnClick_ReadyButton()
    {
        if (GetViewPV().IsMine)
        {
            //presenter.GetMasterPV().RPC("SetReady", PhotonNetwork.MasterClient, enterOrder);
            presenter.GetPresenterPV().RPC("SetReady", PhotonNetwork.MasterClient, enterOrder);
        }
    }

    public void OnClick_StartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BoardGame");
        }
    }

    public void OnClick_BodyRightButton()
    {
        if (GetViewPV().IsMine)
        {
            int lastIndex = wantBodyIndex;
            ++wantBodyIndex;

            if (wantBodyIndex < 0) // ÀÎµ¦½º¸¦ ¹Ù·ÎÀâ¾ÆÁÜ
                wantBodyIndex = presenter.bodyColorCount - 1;
            else if (presenter.bodyColorCount <= wantBodyIndex)
                wantBodyIndex = 0;


            presenter.GetPresenterPV().RPC("AskBodyColorUpdate", RpcTarget.MasterClient, enterOrder, lastIndex, wantBodyIndex, true);
        }
    }

    public void OnClick_BodyLeftButton()
    {
        if (GetViewPV().IsMine)
        {
            int lastIndex = wantBodyIndex;
            --wantBodyIndex;

            if (wantBodyIndex < 0) // ÀÎµ¦½º¸¦ ¹Ù·ÎÀâ¾ÆÁÜ
                wantBodyIndex = presenter.bodyColorCount - 1;
            else if (presenter.bodyColorCount <= wantBodyIndex)
                wantBodyIndex = 0;

            presenter.GetPresenterPV().RPC("AskBodyColorUpdate", RpcTarget.MasterClient, enterOrder, lastIndex, wantBodyIndex, false);
        }
    }

    public void OnClick_HatRightButton()
    {
        if (GetViewPV().IsMine)
        {
            ++hatIndex;

            if (hatIndex < 0) // ÀÎµ¦½º¸¦ ¹Ù·Î Àâ¾ÆÁÜ
                hatIndex = presenter.hatTypeCount - 1;
            else if (presenter.hatTypeCount <= hatIndex)
                hatIndex = 0;

            presenter.GetPresenterPV().RPC("AskHatUpdate", RpcTarget.MasterClient, enterOrder, hatIndex);
        }
    }

    public void OnClick_HatLeftButton()
    {
        if (GetViewPV().IsMine)
        {
            --hatIndex;

            if (hatIndex < 0) // ÀÎµ¦½º¸¦ ¹Ù·Î Àâ¾ÆÁÜ
                hatIndex = presenter.hatTypeCount - 1;
            else if (presenter.hatTypeCount <= hatIndex)
                hatIndex = 0;

            presenter.GetPresenterPV().RPC("AskHatUpdate", RpcTarget.MasterClient, enterOrder, hatIndex);
        }
    }

    public void OnClick_ConfirmButton()
    {
        if (GetViewPV().IsMine)
        {
            selectCanvas.enabled = true;
            customizeCanvas.enabled = false;
        }
    }

    public void OnClick_LeaveRoom()
    {
        if (GetViewPV().IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                presenter.GetPresenterPV().RPC("KickEveryeoneOut", RpcTarget.MasterClient);
            }
            else
            {
                presenter.GetPresenterPV().RPC("MakePlayerLeave", RpcTarget.MasterClient, enterOrder);
            }
            
            presenter.LeaveRoom();
        }
    }

    #endregion

    

}
