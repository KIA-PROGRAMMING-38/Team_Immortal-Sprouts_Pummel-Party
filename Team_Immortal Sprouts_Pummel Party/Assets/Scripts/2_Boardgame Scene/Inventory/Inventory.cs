using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory
{
    public Inventory(BoardgamePlayer owner) => Owner = owner;
    public BoardgamePlayer Owner;

    public UnityEvent<Inventory> OnInventoryInit = new UnityEvent<Inventory>();
    public UnityEvent OnInventoryUpdate = new UnityEvent();

    public List<BelongingItemData> PlayerInventory = new List<BelongingItemData>();
    private Dictionary<ItemData, BelongingItemData> itemDictionary = new Dictionary<ItemData, BelongingItemData>();

    /// <summary>
    /// 인벤토리에 초기 데이터 저장
    /// </summary>
    public void InitInventory()
    {
        if(PlayerInventory.Count == 0)
        {
            ItemData[] items = ItemProvider.ItemTable;
            
            for(int id = 0; id < items.Length; ++id)
            {
                ItemData itemData = items[id];
                BelongingItemData newItem = new BelongingItemData(itemData);
                PlayerInventory.Add(newItem);
                itemDictionary.Add(itemData, newItem);
            }

            OnInventoryInit?.Invoke(this);
            OnInventoryUpdate?.Invoke();
        }   
    }

    /// <summary>
    /// 인벤토리에 아이템 저장
    /// </summary>
    public void Add(ItemData itemData)
    {
        if(itemDictionary.TryGetValue(itemData, out BelongingItemData item))
        {
            item.AddToInventory();
            OnInventoryUpdate?.Invoke();

            Debug.Log($"[{Owner.name}]Inventory 업데이트: {itemData.Name} -> {item.Number}");
        }
        else
        {
            Debug.Log("잘못된 ItemData");
        }
    }

    /// <summary>
    /// 인벤토리에서 아이템 제거 (1개 사용)
    /// </summary>
    public void Remove(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out BelongingItemData item))
        {
            item.RemoveFromInventory();
            OnInventoryUpdate?.Invoke();
        }
    }
}
