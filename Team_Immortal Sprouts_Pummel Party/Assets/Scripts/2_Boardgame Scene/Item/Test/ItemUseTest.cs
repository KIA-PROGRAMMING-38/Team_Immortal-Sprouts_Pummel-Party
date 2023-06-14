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

        // ������ ��� �׽�Ʈ
        //IUsable item = Instantiate(prefab).GetComponent<IUsable>();
        //item.SetForUse(player);

        // ������ �߰� �׽�Ʈ
        prefab.GetComponent<Item>().Get(player);

    }

    private void CallTimeOut()
    {
        OnTimeOut.Invoke();
    }
}
