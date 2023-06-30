using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Item : MonoBehaviour, IUsable
{
    public ItemData ItemData;

    public abstract void SetForUse(BoardPlayerController player = null);

    public virtual void Use() { }
}
