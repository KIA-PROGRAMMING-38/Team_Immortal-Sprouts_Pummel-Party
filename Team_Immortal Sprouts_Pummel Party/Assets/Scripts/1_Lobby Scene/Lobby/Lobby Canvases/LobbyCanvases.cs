using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvases : MonoBehaviour
{
    [SerializeField] private MultiGameCanvas _multiGameCanvas;
    [SerializeField] private Create_Or_Find_RoomCanvas _create_Or_Find_RoomCanvas;
    //[SerializeField] private FindRoomCanvas _findRoomCanvas;
    [SerializeField] private FailedJoinRoomCanvas _failedJoinRoomCanvas;
    //[SerializeField] private WaitingRoomCanvas _waitingRoomCanvas;

    public MultiGameCanvas MultiGameCanvas { get { return _multiGameCanvas; } }
    public Create_Or_Find_RoomCanvas Create_Or_Find_RoomCanvas { get { return _create_Or_Find_RoomCanvas; } }
    //public FindRoomCanvas FindRoomCanvas { get { return _findRoomCanvas; } }
    public FailedJoinRoomCanvas FailedJoinRoomCanvas { get { return _failedJoinRoomCanvas; } }
    //public WaitingRoomCanvas WaitingRoomCanvas { get { return _waitingRoomCanvas; } }

    private void Awake()
    {
        CanvasInitialize();
    }

    private void CanvasInitialize()
    {
        MultiGameCanvas.CanvasInitialize(this);
        Create_Or_Find_RoomCanvas.CanvasInitialize(this);
        //FindRoomCanvas.CanvasInitialize(this);
        FailedJoinRoomCanvas.CanvasInitialize(this);
        //WaitingRoomCanvas.CanvasInitialize(this);
    }

    /// <summary>
    /// Lobby Canvas들을 모두 비활성화
    /// </summary>
    public void DeactiveLobbyCanvases()
    {
        MultiGameCanvas.Deactive();
        Create_Or_Find_RoomCanvas.Deactive();
        //FindRoomCanvas.Deactive();
    }
}
