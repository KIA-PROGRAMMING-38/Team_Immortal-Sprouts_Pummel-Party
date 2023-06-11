using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvases : MonoBehaviour
{
    [SerializeField] private MultiGameCanvas _multiGameCanvas;
    [SerializeField] private CreateRoomCanvas _createRoomCanvas;
    [SerializeField] private FindRoomCanvas _findRoomCanvas;
    [SerializeField] private WaitingRoomCanvas _waitingRoomCanvas;

    public MultiGameCanvas MultiGameCanvas { get { return _multiGameCanvas; } }
    public CreateRoomCanvas CreateRoomCanvas { get { return _createRoomCanvas; } }
    public FindRoomCanvas FindRoomCanvas { get { return _findRoomCanvas; } }
    public WaitingRoomCanvas WaitingRoomCanvas { get { return _waitingRoomCanvas; } }

    private void Awake()
    {
        CanvasInitialize();
    }

    private void CanvasInitialize()
    {
        MultiGameCanvas.CanvasInitialize(this);
        CreateRoomCanvas.CanvasInitialize(this);
        FindRoomCanvas.CanvasInitialize(this);
        WaitingRoomCanvas.CanvasInitialize(this);
    }

    /// <summary>
    /// Lobby Canvas들을 모두 비활성화
    /// </summary>
    public void DeactiveLobbyCanvases()
    {
        MultiGameCanvas.Deactive();
        CreateRoomCanvas.Deactive();
        FindRoomCanvas.Deactive();
    }
}
