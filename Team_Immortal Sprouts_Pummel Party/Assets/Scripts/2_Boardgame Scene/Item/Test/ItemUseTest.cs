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

        // ������ ��� �׽�Ʈ
        //IUsable item = Instantiate(prefab).GetComponent<IUsable>();
        //item.SetForUse(player);

        // ������ �߰� �׽�Ʈ
        Invoke(nameof(GetTest), 5f);
        Invoke(nameof(OtherPlayerGet), 0.5f);
        Invoke(nameof(GetTest), 0.5f);

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
