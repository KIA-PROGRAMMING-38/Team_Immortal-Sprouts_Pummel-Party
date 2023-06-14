using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Cysharp.Threading.Tasks;

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
    [SerializeField] private int enterOrder;

    private Color readyColor = Color.green;
    private Color notReadyColor = Color.red;
    [SerializeField] private int wantBodyIndex = 0;
    [SerializeField] private int hatIndex = 0;

    [SerializeField] private bool isChangable = true;



    #region Public ÇÔ¼öµé

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
        await UniTask.Delay(500); // 0.5ÃÊÀÇ µô·¹ÀÌ
        isChangable = true;
    }

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
            readyBar.color = readyColor;
        }
        else
        {
            readyBar.color = notReadyColor;
        }
    }

    #endregion

    #region OnClick Event ÇÔ¼öµé


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

    public void OnClick_StartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            presenter.LoadBoardGame();
        }
    }

    public void OnClick_BodyRightButton()
    {
        if (GetViewPV().IsMine && isChangable)
        {
            int lastIndex = wantBodyIndex;
            ++wantBodyIndex;

            if (wantBodyIndex < 0) // ÀÎµ¦½º¸¦ ¹Ù·ÎÀâ¾ÆÁÜ
                wantBodyIndex = presenter.bodyColorCount - 1;
            else if (presenter.bodyColorCount <= wantBodyIndex)
                wantBodyIndex = 0;
            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskBodyColorUpdate", RpcTarget.MasterClient, enterOrder, lastIndex, wantBodyIndex, true);
            
        }
    }

    public void OnClick_BodyLeftButton()
    {
        if (GetViewPV().IsMine && isChangable)
        {
            int lastIndex = wantBodyIndex;
            --wantBodyIndex;

            if (wantBodyIndex < 0) // ÀÎµ¦½º¸¦ ¹Ù·ÎÀâ¾ÆÁÜ
                wantBodyIndex = presenter.bodyColorCount - 1;
            else if (presenter.bodyColorCount <= wantBodyIndex)
                wantBodyIndex = 0;

            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskBodyColorUpdate", RpcTarget.MasterClient, enterOrder, lastIndex, wantBodyIndex, false);
            
        }
    }

    public void OnClick_HatRightButton()
    {
        if (GetViewPV().IsMine && isChangable)
        {
            ++hatIndex;

            if (hatIndex < 0) // ÀÎµ¦½º¸¦ ¹Ù·Î Àâ¾ÆÁÜ
                hatIndex = presenter.hatTypeCount - 1;
            else if (presenter.hatTypeCount <= hatIndex)
                hatIndex = 0;

            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskHatUpdate", RpcTarget.MasterClient, enterOrder, hatIndex);
        }
    }

    public void OnClick_HatLeftButton()
    {
        if (GetViewPV().IsMine && isChangable)
        {
            --hatIndex;

            if (hatIndex < 0) // ÀÎµ¦½º¸¦ ¹Ù·Î Àâ¾ÆÁÜ
                hatIndex = presenter.hatTypeCount - 1;
            else if (presenter.hatTypeCount <= hatIndex)
                hatIndex = 0;

            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskHatUpdate", RpcTarget.MasterClient, enterOrder, hatIndex);
        }
    }


    private string setNickName;
    public void OnClick_ConfirmButton()
    {
        if (GetViewPV().IsMine)
        {
            selectCanvas.enabled = true;
            customizeCanvas.enabled = false;
            setNickName = nickNameInputField.text;
            presenter.GetPresenterPV().RPC("SetPlayerNickName", RpcTarget.MasterClient, enterOrder, setNickName);
        }
    }



    #endregion

    [PunRPC]
    private void ShowPlayerNickName(string setNickName)
    {
        playerNickName.text = setNickName;
    }
    

}
