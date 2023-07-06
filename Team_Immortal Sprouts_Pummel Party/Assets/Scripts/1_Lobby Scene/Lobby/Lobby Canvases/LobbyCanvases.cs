using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvases : MonoBehaviourPunCallbacks
{
    [SerializeField] private MultiGameCanvas _multiGameCanvas;
    [SerializeField] private CreateOrFind_RoomCanvas _create_Or_Find_RoomCanvas;
    [SerializeField] private FailedJoinRoomCanvas _failedJoinRoomCanvas;

    public MultiGameCanvas MultiGameCanvas { get { return _multiGameCanvas; } }
    public CreateOrFind_RoomCanvas Create_Or_Find_RoomCanvas { get { return _create_Or_Find_RoomCanvas; } }
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

    public void LoadBoardGame() // 현재는 로비 캔버스에 있지만, 나중에 LoadManager 한테 가줘야함
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo updatedRoomInfo in roomList)
        {
            string updatedRoomName = updatedRoomInfo.Name;
            bool isNewlyCreated;
            if (updatedRoomInfo.RemovedFromList) // 방이 삭제되었다면
            {
                isNewlyCreated = false;
                Managers.DataManager.Room.UpdateRoomData(isNewlyCreated, updatedRoomName);
            }
            else // 방이 삭제 된게 아니라면
            {
                if (!Managers.DataManager.Room.CheckIfRoomExist(updatedRoomName)) // 새로 생성된 방이라면
                {
                    isNewlyCreated = true;
                    Managers.DataManager.Room.UpdateRoomData(isNewlyCreated, updatedRoomName, updatedRoomInfo);
                }
            }
        }

    }
}
