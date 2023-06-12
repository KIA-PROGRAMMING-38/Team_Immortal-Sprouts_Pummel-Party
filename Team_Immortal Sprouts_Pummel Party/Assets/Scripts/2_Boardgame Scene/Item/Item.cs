using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Item : MonoBehaviour, IUsable
{
    public ItemData ItemData;

    /// <summary>
    /// �κ��丮 ���� �� ���� ���� (�ο�)
    /// </summary>
    public virtual void Get(BoardgamePlayer player)
    {

    }

    public virtual void SetForUse(BoardgamePlayer usePlayer) { }

    public virtual void Use() { }
}
