using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvases : MonoBehaviourPunCallbacks
{
    [SerializeField] private MultiGameCanvas _multiGameCanvas;
    [SerializeField] private Create_Or_Find_RoomCanvas _create_Or_Find_RoomCanvas;

    public MultiGameCanvas MultiGameCanvas { get { return _multiGameCanvas; } }
    public Create_Or_Find_RoomCanvas Create_Or_Find_RoomCanvas { get { return _create_Or_Find_RoomCanvas; } }

    

    private void Awake()
    {
        CanvasInitialize();
    }

    private void CanvasInitialize()
    {
        MultiGameCanvas.CanvasInitialize(this);
        Create_Or_Find_RoomCanvas.CanvasInitialize(this);
    }

    /// <summary>
    /// Lobby Canvas들을 모두 비활성화
    /// </summary>
    public void DeactiveLobbyCanvases()
    {
        MultiGameCanvas.Deactive();
        Create_Or_Find_RoomCanvas.Deactive();
    }


    public int GetRoomCount() => entireRooms.Count;
    

    private Dictionary<string, RoomInfo> entireRooms = new Dictionary<string, RoomInfo>();

    public bool CheckIfRoomExist(string roomName)
    {
        bool isExist = false;
        if (entireRooms.ContainsKey(roomName))
        {
            isExist = true;
        }

        return isExist;
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) // 삭제가 된다면
            {
                entireRooms.Remove(roomInfo.Name);
            }
            else
            {
                if (!entireRooms.ContainsKey(roomInfo.Name)) // 새로 생성된 방이라면
                {
                    entireRooms.Add(roomInfo.Name, roomInfo);
                }
            }
        }
    }
}
