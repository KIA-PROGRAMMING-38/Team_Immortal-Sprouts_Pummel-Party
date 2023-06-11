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

    public void ResetReadyCount()
    {
        readyCount = 0;
    }

    public SelectCanvas GetSelectCanvas()
    {
        return selectCanvas;
    }

    public int GetReadyCount()
    {
        return readyCount;
    }

    public PhotonView GetPhotonView()
    {
        return photonView;
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

    private void EnableStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            selectCanvas.CheckAndEnableStartButton(readyCount);
        }
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
    public void EnableSelectCanvasButtons()
    {
        selectCanvas.EnableButtons();
    }

    public CustomizeCanvas GetCustomizeCanvas()
    {
        return customizeCanvas;
    }

}
