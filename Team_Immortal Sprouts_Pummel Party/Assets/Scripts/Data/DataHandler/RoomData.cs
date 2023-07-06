using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    private Dictionary<string, RoomInfo> roomDict = new Dictionary<string, RoomInfo>();

    public int Count{ get { return roomDict.Count; } } // 전체 방 개수

    /// <summary>
    /// 방 정보를 업데이트하는 함수
    /// </summary>
    /// <param name="isRoomAdded"></param>
    /// <param name="roomName"></param>
    /// <param name="roomInfo"></param>
    public void UpdateRoomData(bool isRoomAdded, string roomName, RoomInfo roomInfo = null)
    {
        if (isRoomAdded)
        {
            roomDict.Add(roomName, roomInfo);
        }
        else
        {
            roomDict.Remove(roomName);
        }
    }

    public bool CheckIfRoomExist(string roomName)
    {
        bool isExist = false;
        if (roomDict.ContainsKey(roomName))
        {
            isExist = true;
        }

        return isExist;
    }
}
