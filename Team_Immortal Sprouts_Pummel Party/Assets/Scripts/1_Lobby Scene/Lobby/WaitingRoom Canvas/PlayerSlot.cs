using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviourPunCallbacks
{
    // WaitingRoomCanvas OnJoined, OnPlayerEntered, OnPlayerLeft, OnMasterSwitched

    // PlayerSlot
    // View(UI) : StatusBar / Select Canvas -> 닉네임, 커스터마이즈 버튼, 스타트 버튼 / Customize Canvas -> 플레이어닉네임 인풋필드, 몸색변경 버튼, 모자 변경 버튼, 확정 버튼, 몸색, 모자
    // Presenter(중간매개체) : WaitingRoomCanvas 가 중간 매개체 역할, Model 한테서 정보를 받아서 View를 컨트롤 한다
    // Model(Data) : LobbyPlayerData 플레이어 정보, 몇번째 플레이어? , 플레이어 닉네임, 현재 몸 색깔 정보, 현재 모자 정보



    [SerializeField] Image statusBar;
    [SerializeField] SelectCanvas selectCanvas;
    [SerializeField] Canvas selectCanvasPower;
    [SerializeField] CustomizeCanvas customizeCanvas;
    [SerializeField] Canvas customizeCanvasPower;
    

    [SerializeField] private int readyCount; // 테스트 위해 SerializeField 추가함
    [SerializeField] private PhotonView photonView;

    private Color readyColor = Color.green;
    private Color notReadyColor = Color.red;

    
    public CustomizeCanvas GetCustomizeCanvas() => customizeCanvas;
    public SelectCanvas GetSelectCanvas() => selectCanvas;

    [SerializeField] private PlayerModelChanger playerModelChanger; // 테스트 위해 SerializeField

    

    


    public void SetPlayerModelChanger(PlayerModelChanger modelChanger)
    {
        playerModelChanger = modelChanger;
    }

    public PlayerModelChanger GetPlayerModelChanger()
    {
        return playerModelChanger;
    }
    /// <summary>
    /// 매개변수에 따라 플레이어의 Customize Canvas를 키고, Select Canvas를 꺼준다
    /// </summary>
    public void ActivateCustomizeCanvas(bool isTurnOn)
    {
        if (isTurnOn)
        {
            customizeCanvasPower.enabled = true;
            selectCanvasPower.enabled = false;
        }
        else
        {
            customizeCanvasPower.enabled = false;
            selectCanvasPower.enabled = true;
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
