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
        Debug.Log("GameStart ��ư�� Ŭ����");

        // ���� ���ٸ� �������� ���� �����ؼ� ���Ƿ� �̵�
        // ���� �ִٸ� �� ������ ����
        // ���⼭ �����Ǵ� ���� ���� ����

    }
}
