using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Item : MonoBehaviour, IUsable
{
    public ItemData ItemData;
    //public virtual void SetForUse(BoardgamePlayer usePlayer) { }

    public virtual void Use() { }
}
