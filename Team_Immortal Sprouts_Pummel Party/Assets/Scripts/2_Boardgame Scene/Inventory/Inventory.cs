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
        // TODO: GameManager ����Ǵ� ��� static class���� �ش� ���� �ϴ� �޼ҵ� ����� ȣ���ϱ� -> �о�� ItemData��� Awake or Start���� new InventoryItem()
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

            Debug.Log($"Inventory ������Ʈ: {itemData.Name} -> {item.Number}");
        }
        else
        {
            Debug.Log("�߸��� ItemData");
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
