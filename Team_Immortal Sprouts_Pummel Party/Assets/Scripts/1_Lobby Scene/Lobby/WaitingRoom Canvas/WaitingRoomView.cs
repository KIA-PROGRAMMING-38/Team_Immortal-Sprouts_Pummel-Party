using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Cysharp.Threading.Tasks;
using System;
using Photon.Realtime;
using Photon.Pun.Demo.Cockpit;

public class WaitingRoomView : MonoBehaviourPunCallbacks
{
    [SerializeField] private WaitingRoomPresenter presenter;

    [Header("----------------------Select Canvas----------------------")]
    [SerializeField] private Canvas selectCanvas;
    [SerializeField] private Image readyBar;
    [SerializeField] private TMP_Text playerNickName;
    [SerializeField] private Button startButton;

    [Header("----------------------Customize Canvas----------------------")]
    [SerializeField] private Canvas customizeCanvas;
    [SerializeField] private TMP_Text nickNameInputField;
    [SerializeField] private Image colorBackground;
    [SerializeField] private TMP_Text hatShowText;


    [Header("----------------------Editor Mode----------------------")]
    PhotonView viewPV;
    [field : SerializeField] public int enterOrder { get; set; }
    [field: SerializeField] public bool isChangable { get; private set; }


    private void Awake()
    {
        isChangable = true;
        viewPV = GetComponent<PhotonView>();
    }

    #region Public 함수들

    [PunRPC]
    public void SetBackgroundColor(int colorIndex)
    {
        colorBackground.color = presenter.GetBackgroundColor(colorIndex);
    }

    [PunRPC]
    public void SetHatText(int hatIndex)
    {
        hatShowText.text = presenter.GetBackgroundHatText(hatIndex);    
    }

    [PunRPC]
    public void ActivateStartButton(bool isAllReady)
    {
        startButton.interactable = isAllReady;
    }
    
    private async UniTaskVoid EnableIsChangable()
    {
        isChangable = false;
        await UniTask.Delay(500); // 0.5초의 딜레이로 광클 예방
        isChangable = true;
    }

    public PhotonView GetViewPV()
    {
        //if (viewPV == null)
        //{
        //    viewPV = GetComponent<PhotonView>();
        //}

        return viewPV;
    }


    [PunRPC]
    public void SetEnterOrder(int myEnterOrder)
    {
        enterOrder = myEnterOrder;
    }

    public event Func<bool, Color> OnClickReadyButton;
    [PunRPC]
    public void SetReadyColor(bool isReady)
    {
        if (isReady == true)
        {
            readyBar.color = (Color)OnClickReadyButton?.Invoke(isReady);
        }
        else
        {
            readyBar.color = (Color)OnClickReadyButton?.Invoke(isReady);
        }
    }

    #endregion

    #region OnClick Event 함수들


    public void OnClick_CustomizeButton()
    {
        if ( enterOrder != 0 && GetViewPV().IsMine)
        {
            selectCanvas.enabled = false;
            customizeCanvas.enabled = true;
        }
    }

    public void OnClick_ReadyButton()
    {
        if (GetViewPV().IsMine)
        {
            presenter.GetPresenterPV().RPC("SetReady", PhotonNetwork.MasterClient, enterOrder);
        }
    }

    public void OnClick_StartButton() // 방장 버튼에만 할당
    {
        if (PhotonNetwork.IsMasterClient)
        {
            presenter.MoveToBoardGame();
        }
    }

    public void OnClick_BodyRightButton()
    {
        if (GetViewPV().IsMine && isChangable)
        {
            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskBodyColorUpdate", RpcTarget.MasterClient, enterOrder, true, false);
        }
    }

    public void OnClick_BodyLeftButton()
    {
        if (GetViewPV().IsMine && isChangable)
        {
            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskBodyColorUpdate", RpcTarget.MasterClient, enterOrder, false, false);

        }
    }

    public void OnClick_HatRightButton()
    {
        if (GetViewPV().IsMine && isChangable)
        {
            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskHatUpdate", RpcTarget.MasterClient, enterOrder, true);
        }
    }

    public void OnClick_HatLeftButton()
    {
        if (GetViewPV().IsMine && isChangable)
        {
            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskHatUpdate", RpcTarget.MasterClient, enterOrder, false);
        }
    }

    public void OnClick_ConfirmButton()
    {
        if (GetViewPV().IsMine)
        {
            selectCanvas.enabled = true;
            customizeCanvas.enabled = false;
            Player player = Managers.DataManager.Player.GetPhotonPlayer(enterOrder);
            string nickName = nickNameInputField.text;
            Managers.DataManager.Player.SetNickName(player, nickName);
            presenter.GetPresenterPV().RPC("SetPlayerNickName", RpcTarget.MasterClient, enterOrder, nickName);
        }
    }

    #endregion

    [PunRPC]
    private void ShowPlayerNickName(string setNickName)
    {
        playerNickName.text = setNickName;
    }
    

}
