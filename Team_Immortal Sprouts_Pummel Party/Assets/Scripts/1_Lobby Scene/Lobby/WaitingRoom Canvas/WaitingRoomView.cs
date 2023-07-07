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
    
    [field : SerializeField] public int wantBodyIndex { get; set; }
    [field : SerializeField] public int hatIndex { get; set; }
    [field: SerializeField] public bool isChangable { get; private set; }


    private void Awake()
    {
        isChangable = true;
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
    public void SetEnterOrder(int myEnterOrder)
    {
        enterOrder = myEnterOrder;
        wantBodyIndex = myEnterOrder;
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
            Player myPlayer = Managers.DataManager.Player.GetPhotonPlayer(enterOrder);
            int lastIndex = Managers.DataManager.Player.GetBodyID(myPlayer);
            int desiredIndex = lastIndex + 1;

            if (desiredIndex < 0)
            {
                desiredIndex = Managers.DataManager.Player.GetBodyTypeCount() - 1;
            }
            else if(Managers.DataManager.Player.GetBodyTypeCount() <= desiredIndex)
            {
                desiredIndex = 0;
            }

            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskBodyColorUpdate", RpcTarget.MasterClient, enterOrder, lastIndex, desiredIndex, true, false);
        }
    }

    public void OnClick_BodyLeftButton()
    {
        if (GetViewPV().IsMine && isChangable)
        {
            Player myPlayer = Managers.DataManager.Player.GetPhotonPlayer(enterOrder);
            int lastIndex = Managers.DataManager.Player.GetBodyID(myPlayer);
            int desiredIndex = lastIndex - 1;
            


            if (desiredIndex < 0)
            {
                desiredIndex = Managers.DataManager.Player.GetBodyTypeCount() - 1;
            }
            else if (Managers.DataManager.Player.GetBodyTypeCount() <= desiredIndex)
            {
                desiredIndex = 0;
            }


            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskBodyColorUpdate", RpcTarget.MasterClient, enterOrder, lastIndex, desiredIndex, false, false);

        }
    }

    public void OnClick_HatRightButton()
    {
        if (GetViewPV().IsMine && isChangable)
        {
            Player myPlayer = Managers.DataManager.Player.GetPhotonPlayer(enterOrder);
            int lastIndex = Managers.DataManager.Player.GetHatID(myPlayer);
            int desiredIndex = lastIndex + 1;
            hatIndex = desiredIndex;

            if (desiredIndex < 0)
            {
                desiredIndex = Managers.DataManager.Player.GetHatTypeCount() - 1;
            }
            else if (Managers.DataManager.Player.GetHatTypeCount() <= desiredIndex)
            {
                desiredIndex = 0;
            }

            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskHatUpdate", RpcTarget.MasterClient, enterOrder, desiredIndex);
        }
    }

    public void OnClick_HatLeftButton()
    {
        if (GetViewPV().IsMine && isChangable)
        {
            Player myPlayer = Managers.DataManager.Player.GetPhotonPlayer(enterOrder);
            int lastIndex = Managers.DataManager.Player.GetHatID(myPlayer);
            int desiredIndex = lastIndex - 1;
            hatIndex = desiredIndex;

            if (desiredIndex < 0)
            {
                desiredIndex = Managers.DataManager.Player.GetHatTypeCount() - 1;
            }
            else if (Managers.DataManager.Player.GetHatTypeCount() <= desiredIndex)
            {
                desiredIndex = 0;
            }

            EnableIsChangable().Forget();
            presenter.GetPresenterPV().RPC("AskHatUpdate", RpcTarget.MasterClient, enterOrder, desiredIndex);
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
