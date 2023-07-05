using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Item : MonoBehaviour, IUsable
{
    public abstract void OnTimeOut();

    public abstract void Use(BoardPlayerController player = null);
}
