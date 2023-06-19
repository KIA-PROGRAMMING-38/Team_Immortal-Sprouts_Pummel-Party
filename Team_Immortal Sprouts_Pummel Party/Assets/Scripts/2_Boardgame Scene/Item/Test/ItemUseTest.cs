using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemUseTest : MonoBehaviour
{
    public static UnityEvent OnTimeOut;

    public GameObject prefab;
    public BoardgamePlayer player;
    public BoardgamePlayer otherPlayer;

    void Start()
    {
        OnTimeOut = new UnityEvent();

        // 아이템 사용 테스트
        //IUsable item = Instantiate(prefab).GetComponent<IUsable>();
        //item.SetForUse(player);

        // 아이템 추가 테스트
        Invoke(nameof(GetTest), 1f);
        Invoke(nameof(OtherPlayerGet), 2f);
        Invoke(nameof(GetTest), 5f);

    }

    private void GetTest()
    {
        prefab.GetComponent<Item>().Get(player);
    }

    private void OtherPlayerGet()
    {
        prefab.GetComponent<Item>().Get(otherPlayer);
    }

    private void CallTimeOut()
    {
        OnTimeOut.Invoke();
    }
}
