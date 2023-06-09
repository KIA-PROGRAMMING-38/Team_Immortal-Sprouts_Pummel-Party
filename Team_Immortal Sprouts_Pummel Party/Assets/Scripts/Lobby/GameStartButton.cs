using JetBrains.Annotations;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class GameStartButton : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject WaitingRoomCanvas;
    [SerializeField] private GameObject FailedJoinRoomCanvas;

    public void OnClickGameStartButton()
    {
        OnJoinRandomRoom();
        Debug.Log("GameStart 버튼이 클릭됨");
    }

    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    if (roomList.Count == 0)
    //    {
    //        FailedJoinRoomCanvas.SetActive(true);
    //    }
    //}

    public override void OnJoinedRoom()
    {
        WaitingRoomCanvas.gameObject.SetActive(true);
        Debug.Log("방에 입장하였습니다.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        FailedJoinRoomCanvas.SetActive(true);
        Debug.Log($"{message}의 이유로 방 입장에 실패하였습니다");
    }

    private void OnJoinRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
}
