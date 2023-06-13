using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviourPunCallbacks
{
    // WaitingRoomCanvas OnJoined, OnPlayerEntered, OnPlayerLeft, OnMasterSwitched

    // PlayerSlot
    // View(UI) : StatusBar / Select Canvas -> �г���, Ŀ���͸����� ��ư, ��ŸƮ ��ư / Customize Canvas -> �÷��̾�г��� ��ǲ�ʵ�, �������� ��ư, ���� ���� ��ư, Ȯ�� ��ư, ����, ����
    // Presenter(�߰��Ű�ü) : WaitingRoomCanvas �� �߰� �Ű�ü ����, Model ���׼� ������ �޾Ƽ� View�� ��Ʈ�� �Ѵ�
    // Model(Data) : LobbyPlayerData �÷��̾� ����, ���° �÷��̾�? , �÷��̾� �г���, ���� �� ���� ����, ���� ���� ����



    [SerializeField] Image statusBar;
    [SerializeField] SelectCanvas selectCanvas;
    [SerializeField] Canvas selectCanvasPower;
    [SerializeField] CustomizeCanvas customizeCanvas;
    [SerializeField] Canvas customizeCanvasPower;
    

    [SerializeField] private int readyCount; // �׽�Ʈ ���� SerializeField �߰���
    [SerializeField] private PhotonView photonView;

    private Color readyColor = Color.green;
    private Color notReadyColor = Color.red;

    
    public CustomizeCanvas GetCustomizeCanvas() => customizeCanvas;
    public SelectCanvas GetSelectCanvas() => selectCanvas;

    [SerializeField] private PlayerModelChanger playerModelChanger; // �׽�Ʈ ���� SerializeField

    

    


    public void SetPlayerModelChanger(PlayerModelChanger modelChanger)
    {
        playerModelChanger = modelChanger;
    }

    public PlayerModelChanger GetPlayerModelChanger()
    {
        return playerModelChanger;
    }
    /// <summary>
    /// �Ű������� ���� �÷��̾��� Customize Canvas�� Ű��, Select Canvas�� ���ش�
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
    /// ������ �����ִ� readyCount�� 0���� �������ش�
    /// </summary>
    public void ResetReadyCount()
    {
        readyCount = 0;
    }

    /// <summary>
    /// ������ �����ִ� �÷��̾���� readyCount�� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public int GetReadyCount() => readyCount;

    /// <summary>
    /// PlayerSlot�� �Ҵ�� photonView �� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public PhotonView GetPhotonView() => photonView;
    


    #region PunRPC �Լ�

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
