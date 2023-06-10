using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRoomFailedPanel : MonoBehaviour
{
    /// <summary>
    /// 방 찾기 실패를 알리는 UI Panel을 활성화
    /// </summary>
    public void Active()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 방 찾기 실패를 알리는 UI Panel을 비활성화
    /// </summary>
    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 방 찾기 실패 Panel의 OK 버튼 이벤트
    /// </summary>
    public void OnClick_OK()
    {
        Deactive();
    }
}
