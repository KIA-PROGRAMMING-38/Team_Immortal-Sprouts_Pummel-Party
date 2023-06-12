using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviourPunCallbacks
{
    [SerializeField] Image statusBar;
    [SerializeField] SelectCanvas selectCanvas;
    [SerializeField] CustomizeCanvas customizeCanvas;

    [SerializeField] private int readyCount; // �׽�Ʈ ���� SerializeField �߰���
    [SerializeField] private PhotonView photonView;

    private Color readyColor = Color.green;
    private Color notReadyColor = Color.red;

    
    public CustomizeCanvas GetCustomizeCanvas() => customizeCanvas;
    public SelectCanvas GetSelectCanvas() => selectCanvas;


    /// <summary>
    /// �Ű������� ���� �÷��̾��� Customize Canvas�� Ű��, Select Canvas�� ���ش�
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
    /// �Ű������� ���� �÷��̾��� Select Canvas�� Ű��, Customize Canvas�� ���ش� 
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
