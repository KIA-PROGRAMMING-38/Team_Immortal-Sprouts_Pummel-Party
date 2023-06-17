using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory
{
    public static UnityEvent<List<InventoryItem>> OnInventoryUpdate = new UnityEvent<List<InventoryItem>>();
    public static UnityEvent<List<InventoryItem>> OnInventoryInit = new UnityEvent<List<InventoryItem>>();

    public List<InventoryItem> PlayerInventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> _itemDictionary = new Dictionary<ItemData, InventoryItem>();

    public void InitInventory()
    {
        // TODO: GameManager 연결되는 경우 static class에서 해당 동작 하는 메소드 만들고 호출하기 -> 읽어온 ItemData들로 Awake or Start에서 new InventoryItem()
        if(PlayerInventory.Count == 0)
        {
            ItemData[] items = Resources.LoadAll<ItemData>("ItemData");
            
            for(int id = 0; id < items.Length; ++id)
            {
                ItemData itemData = items[id];
                InventoryItem newItem = new InventoryItem(itemData);
                PlayerInventory.Add(newItem);
                _itemDictionary.Add(itemData, newItem);
            }

            OnInventoryInit?.Invoke(PlayerInventory);
            OnInventoryUpdate?.Invoke(PlayerInventory);
        }   
    }

    public void Add(ItemData itemData)
    {
        if(_itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToInventory();
            OnInventoryUpdate?.Invoke(PlayerInventory);

            Debug.Log($"Inventory 업데이트: {itemData.Name} -> {item.Number}");
        }
        else
        {
            Debug.Log("잘못된 ItemData");
        }
    }

    public void Remove(ItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromInventory();
            OnInventoryUpdate?.Invoke(PlayerInventory);
        }
    }
}
