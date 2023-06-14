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

        IUsable item = Instantiate(prefab).GetComponent<IUsable>();
        item.SetForUse(player);
    }

    private void CallTimeOut()
    {
        OnTimeOut.Invoke();
    }
}
