using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvases : MonoBehaviour
{
    [SerializeField] private MultiGameCanvas _multiGameCanvas;
    [SerializeField] private CreateRoomCanvas _createRoomCanvas;
    [SerializeField] private Canvas _waitingRoomCanvas;

    public MultiGameCanvas MultiGameCanvas { get { return _multiGameCanvas; } }
    public CreateRoomCanvas CreateRoomCanvas { get { return _createRoomCanvas; } }
    public Canvas WaitingRoomCanvas { get { return _waitingRoomCanvas; } }

    private void Awake()
    {
        CanvasInitialize();
    }

    private void CanvasInitialize()
    {
        MultiGameCanvas.CanvasInitialize(this);
        CreateRoomCanvas.CanvasInitialize(this);
    }
}
