using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RealItem : MonoBehaviour, IUsable
{
    private int ID;
    private string name;
    private Sprite icon;
    private string description;
    public void SetItemInfo(ItemData itemData, int id)
    {
        ID = itemData.GetItemID(id);
        name = itemData.GetItemName(id);
        icon = itemData.GetItemIcon(id);
        description = itemData.GetItemDescription(id);
    }

    public int GetId() => ID;
    public string GetName() => name;
    public Sprite GetIcon() => icon;    
    public string GetDescription() => description;

    public abstract void Use(BoardPlayerController player = null);
    public abstract void OnTimeOut();
}
