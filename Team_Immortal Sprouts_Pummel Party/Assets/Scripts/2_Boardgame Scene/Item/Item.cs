using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Item : MonoBehaviour, IUsable
{
    public ItemData ItemData;

    /// <summary>
    /// 인벤토리 구현 후 구현 예정 (민영)
    /// </summary>
    public virtual void Get(BoardgamePlayer player)
    {

    }

    public virtual void SetForUse(BoardgamePlayer usePlayer) { }

    public virtual void Use() { }
}
