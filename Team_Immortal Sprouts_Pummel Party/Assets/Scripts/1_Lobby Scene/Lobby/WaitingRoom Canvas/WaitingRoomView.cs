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

    #region Unity 이벤트 

    public UnityEvent<int, Image> OnClickReadyButton;
    public UnityEvent OnClickStartButton;



    #endregion


    #region Public 함수들
    public PhotonView GetViewPV()
    {
        if (viewPV == null)
        {
            viewPV = GetComponent<PhotonView>();    
        }

        return viewPV;  
    }
    
    [PunRPC]
    public void SetEnterOrder(int enterOrder)
    {
        this.enterOrder = enterOrder;
    }
    

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

    #region OnClick Event 함수들


    public void OnClick_CustomizeButton()
    {
        if (viewPV.IsMine)
        {
            selectCanvas.enabled = false;
            customizeCanvas.enabled = true;
        }
    }
    public void OnClick_ReadyButton()
    {
        if(viewPV.IsMine)
        {
            OnClickReadyButton?.Invoke(enterOrder, readyBar); // 들어온 순서와 레디바를 넘겨준다
        }
    }

    public void OnClick_StartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            OnClickStartButton?.Invoke();
        }
    }


    #endregion


}
