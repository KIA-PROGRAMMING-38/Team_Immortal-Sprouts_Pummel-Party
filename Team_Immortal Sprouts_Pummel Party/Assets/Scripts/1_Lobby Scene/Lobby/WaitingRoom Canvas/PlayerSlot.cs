using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviourPunCallbacks
{
    [SerializeField] Image statusBar;
    [SerializeField] SelectCanvas selectCanvas;
    [SerializeField] CustomizeCanvas customizeCanvas;

    [SerializeField] private int readyCount; // 테스트 위해 SerializeField 추가함
    [SerializeField] private PhotonView photonView;

    private Color readyColor = Color.green;
    private Color notReadyColor = Color.red;

    
    public CustomizeCanvas GetCustomizeCanvas() => customizeCanvas;
    public SelectCanvas GetSelectCanvas() => selectCanvas;


    /// <summary>
    /// 매개변수에 따라 플레이어의 Customize Canvas를 키고, Select Canvas를 꺼준다
    /// </summary>
    public void ActivateCustomizeCanvas(bool isTurnOn)
    {
        if (isTurnOn)
        {
            customizeCanvas.gameObject.SetActive(true);
            selectCanvas.gameObject.SetActive(false);
        }
        else
        {
            customizeCanvas.gameObject.SetActive(false);
            selectCanvas.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 매개변수에 따라 플레이어의 Select Canvas를 키고, Customize Canvas를 꺼준다 
    /// </summary>
    /// <param name="isTurnOn"></param>
    public void ActivateSelectCanvas(bool isTurnOn)
    {
        if (isTurnOn)
        {
            selectCanvas.gameObject.SetActive(true);
            customizeCanvas.gameObject.SetActive(false);
        }
        else
        {
            selectCanvas.gameObject.SetActive(false);
            customizeCanvas.gameObject.SetActive(true);
        }
    }




    /// <summary>
    /// 방장이 갖고있는 readyCount를 0으로 리셋해준다
    /// </summary>
    public void ResetReadyCount()
    {
        readyCount = 0;
    }

    /// <summary>
    /// 방장이 갖고있는 플레이어들의 readyCount를 반환한다
    /// </summary>
    /// <returns></returns>
    public int GetReadyCount() => readyCount;

    /// <summary>
    /// PlayerSlot에 할당된 photonView 를 반환한다
    /// </summary>
    /// <returns></returns>
    public PhotonView GetPhotonView() => photonView;
    


    #region PunRPC 함수

    [PunRPC]
    public void EnableSelectCanvasButtons()
    {
        selectCanvas.EnableButtons();
    }

    [PunRPC]
    public void SetReadyColor()
    {
        statusBar.color = readyColor;
    }

    [PunRPC]
    public void SetNotReadyColor()
    {
        if (statusBar != null)
        {
            statusBar.color = notReadyColor;
        }
    }

    [PunRPC]
    public void SetReady()
    {
        ++readyCount;
        EnableStartButton();
    }

    [PunRPC]
    public void UnSetReady()
    {
        --readyCount;
        EnableStartButton();
    }

    #endregion

    private void EnableStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            selectCanvas.CheckAndEnableStartButton(readyCount);
        }
    }
}
