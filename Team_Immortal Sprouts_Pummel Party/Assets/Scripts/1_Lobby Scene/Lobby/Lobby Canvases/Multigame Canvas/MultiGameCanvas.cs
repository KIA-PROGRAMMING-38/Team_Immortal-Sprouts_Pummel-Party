using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiGameCanvas : MonoBehaviour
{
    private LobbyCanvases _lobbyCanvases;
    private CanvasGroup _canvasGroup;
    [SerializeField] private bool isCreatingRoom; // �׽�Ʈ ���� SerializeField �Է�

    /// <summary>
    /// Lobby�� �����ϴ� Canvas���� ���� ������ �� �ֵ��� �ʱ� ����
    /// </summary>
    public void CanvasInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }


    #region OnClick Events

    /// <summary>
    /// ���ӽ�ŸƮ ��ư Ŭ���� �۵��ϴ� �̺�Ʈ �Լ�
    /// </summary>
    public void OnClick_GameStartButton()
    {
        OnJoinRandomRoom();
        Debug.Log("GameStart ��ư�� Ŭ����");
    }

    /// <summary>
    /// Create �Ǵ� Find Room ��ư Ŭ���� �۵��ϴ� �̺�Ʈ �Լ�
    /// </summary>
    public void OnClick_CreateRoom()
    {
        _lobbyCanvases.Create_Or_Find_RoomCanvas.Active();
        isCreatingRoom = true;
        TurnOffRaycast();
    }

    /// <summary>
    /// Find Room ��ư Ŭ���� �۵��ϴ� �̺�Ʈ �Լ�
    /// </summary>
    public void OnClick_FindRoom()
    {
        _lobbyCanvases.Create_Or_Find_RoomCanvas.Active();
        isCreatingRoom = false;
        //_lobbyCanvases.FindRoomCanvas.Active();
        TurnOffRaycast();
    }


    /// <summary>
    /// �������� ��ư Ŭ���� �۵��ϴ� �̺�Ʈ �Լ�
    /// </summary>
    public void OnClick_LeaveGameButton()
    {
        Application.Quit();
        Debug.Log("������ �������ϴ�.");
    }

    #endregion

    /// <summary>
    /// MultiGame Canvas�� raycast �Է��� ����
    /// </summary>
    public void TurnOffRaycast()
    {
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// MultiGame Canvas�� raycast �Է��� ���� �� �ֵ��� ����
    /// </summary>
    public void TurnOnRaycast()
    {
        _canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// MultiGame Canvas�� Ȱ��ȭ
    /// </summary>
    public void Active()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// MultiGame Canvas�� ��Ȱ��ȭ
    /// </summary>
    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    public bool GetIsCreatingRoom()
    {
        return isCreatingRoom;
    }

    private void OnJoinRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
}
