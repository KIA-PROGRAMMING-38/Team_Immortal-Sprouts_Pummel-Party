using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveGame : MonoBehaviour
{
    public void OnClickLeaveGameButton()
    {
        Application.Quit();
        Debug.Log("게임을 떠났습니다.");
    }
}
