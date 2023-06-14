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

    void Start()
    {
        OnTimeOut = new UnityEvent();

        // 아이템 사용 테스트
        //IUsable item = Instantiate(prefab).GetComponent<IUsable>();
        //item.SetForUse(player);

        // 아이템 추가 테스트
        prefab.GetComponent<Item>().Get(player);

    }

    private void CallTimeOut()
    {
        OnTimeOut.Invoke();
    }
}
