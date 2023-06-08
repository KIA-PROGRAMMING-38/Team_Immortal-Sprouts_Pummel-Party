using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvases : MonoBehaviour
{
    [SerializeField] private MultiGameCanvas _multiGameCanvas;
    [SerializeField] private CreateRoomCanvas _createRoomCanvas;

    public MultiGameCanvas MultiGameCanvas { get { return _multiGameCanvas; } }
    public CreateRoomCanvas CreateRoomCanvas { get { return _createRoomCanvas; } }

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
