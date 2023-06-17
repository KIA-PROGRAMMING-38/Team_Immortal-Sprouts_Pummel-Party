using UnityEngine;

[SerializeField]
public class InventoryItem
{
    public ItemData ItemData;
    public int Number;
    public bool IsHolding;

    public InventoryItem(ItemData item)
    {
        ItemData = item;
        Number = 0;
        IsHolding = false;
    }

    public void AddToInventory()
    {
        ++Number;

        if(Number >= 1)
        {
            IsHolding = true;
        }
    }

    public void RemoveFromInventory()
    {
        --Number;

        if(Number == 0)
        {
            IsHolding = false;
        }
    }
}
