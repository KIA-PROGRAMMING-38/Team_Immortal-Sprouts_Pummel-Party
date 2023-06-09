using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiGameCanvas : MonoBehaviour
{
    private LobbyCanvases _lobbyCanvases;
    private CanvasGroup _canvasGroup;

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

    /// <summary>
    /// Create Room ��ư �Է����� ���� �̺�Ʈ
    /// </summary>
    public void OnClick_CreateRoom()
    {
        _lobbyCanvases.CreateRoomCanvas.Active();
        TurnOffRaycast();
    }

    /// <summary>
    /// Find Room ��ư �Է����� ���� �̺�Ʈ
    /// </summary>
    public void OnClick_FindRoom()
    {
        _lobbyCanvases.FindRoomCanvas.Active();
        TurnOffRaycast();
    }

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
}