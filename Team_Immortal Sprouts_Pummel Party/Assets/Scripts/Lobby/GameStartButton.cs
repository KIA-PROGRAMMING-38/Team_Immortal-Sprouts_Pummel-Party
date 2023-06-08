using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButton : MonoBehaviour
{
    [SerializeField] private GameObject WaitingRoomCanvas;

    public void OnClickGameStartButton()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        WaitingRoomCanvas.gameObject.SetActive(true);
        Debug.Log("GameStart 버튼이 클리됨");

        // 방이 없다면 방장으로 방을 생성해서 대기실로 이동
        // 방이 있다면 그 방으로 입장
        // 여기서 생성되는 방은 랜덤 방임

    }
}
