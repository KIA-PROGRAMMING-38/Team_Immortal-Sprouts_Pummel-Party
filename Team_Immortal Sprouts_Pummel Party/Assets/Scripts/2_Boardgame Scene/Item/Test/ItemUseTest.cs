using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseTest : MonoBehaviour
{
    public GameObject prefab;
    public BoardgamePlayer player;

    void Start()
    {
        IUsable item = Instantiate(prefab).GetComponent<IUsable>();
        item.SetForUse(player);
    }
}
