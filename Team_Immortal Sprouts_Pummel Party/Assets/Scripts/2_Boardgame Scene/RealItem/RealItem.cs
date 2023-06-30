using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RealItem : MonoBehaviour, IUsable
{
    public abstract void SetForUse(BoardPlayerController player = null);
    public abstract void Use();
}
