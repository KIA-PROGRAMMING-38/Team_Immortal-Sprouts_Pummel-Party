using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvases : MonoBehaviourPunCallbacks
{
    [SerializeField] private MultiGameCanvas _multiGameCanvas;
    [SerializeField] private Create_Or_Find_RoomCanvas _create_Or_Find_RoomCanvas;
    [SerializeField] private FailedJoinRoomCanvas _failedJoinRoomCanvas;

    public MultiGameCanvas MultiGameCanvas { get { return _multiGameCanvas; } }
    public Create_Or_Find_RoomCanvas Create_Or_Find_RoomCanvas { get { return _create_Or_Find_RoomCanvas; } }
    public FailedJoinRoomCanvas FailedJoinRoomCanvas { get { return _failedJoinRoomCanvas; } }

    

    private void Awake()
    {
        CanvasInitialize();
    }

    private void CanvasInitialize()
    {
        MultiGameCanvas.CanvasInitialize(this);
        Create_Or_Find_RoomCanvas.CanvasInitialize(this);
        FailedJoinRoomCanvas.CanvasInitialize(this);
    }

    /// <summary>
    /// Lobby Canvas들을 모두 비활성화
    /// </summary>
    public void DeactiveLobbyCanvases()
    {
        MultiGameCanvas.Deactive();
        Create_Or_Find_RoomCanvas.Deactive();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            string updatedRoomName = roomInfo.Name;
            bool isNewlyCreated;
            if (roomInfo.RemovedFromList) // 방이 삭제되었다면
            {
                isNewlyCreated = false;
                RootManager.DataManager.Room.UpdateRoomData(isNewlyCreated, updatedRoomName);
            }
            else // 방이 삭제 된게 아니라면
            {
                if (!RootManager.DataManager.Room.CheckIfRoomExist(updatedRoomName)) // 새로 생성된 방이라면
                {
                    isNewlyCreated = true;
                    RootManager.DataManager.Room.UpdateRoomData(isNewlyCreated, updatedRoomName, roomInfo);
                }
            }
        }

        
    }
}
