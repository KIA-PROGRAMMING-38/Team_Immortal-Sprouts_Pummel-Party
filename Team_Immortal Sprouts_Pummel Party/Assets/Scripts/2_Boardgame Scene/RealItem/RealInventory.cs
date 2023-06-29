using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RealInventory
{
    public RealInventory(ItemData itemData)
    {
        inventory = new List<(RealItem item, int count)>();
        for (int i = 0; i < itemData.GetItemNumber() ;++i)
        {
            RealItem storedItem = itemData.GetItemPrefab(i);
            inventory.Add((item : storedItem, count : 0));
        }
    }

    private List<(RealItem item, int count)> inventory;

    public UnityEvent OnInventoryUpdate = new UnityEvent(); 

    /// <summary>
    /// 아이템을 반환하는 함수
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public RealItem GetItem(int itemID) => inventory[itemID].item;

    /// <summary>
    /// 인벤토리에 아이템을 추가하는 함수
    /// </summary>
    /// <param name="itemID"></param>
    public void IncreaseItemCount(int itemID)
    {
        inventory[itemID] = (inventory[itemID].item, inventory[itemID].count + 1); // 어쨰서 inventory[itemID].count++ 은 안되는가...?
        OnInventoryUpdate?.Invoke();
    }

    /// <summary>
    /// 인벤토리에서 아이템을 삭제하는 함수
    /// </summary>
    /// <param name="itemID"></param>
    public void DecreaseItemCount(int itemID)
    {
        inventory[itemID] = (inventory[itemID].item, Mathf.Max(0, inventory[itemID].count - 1));
        OnInventoryUpdate?.Invoke();
    }

    
}
