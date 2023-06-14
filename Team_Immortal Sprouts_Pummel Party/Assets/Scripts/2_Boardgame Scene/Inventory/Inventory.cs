using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public UnityEvent<List<InventoryItem>> OnInventoryUpdate;
    public UnityEvent OnInventoryInit;

    public List<InventoryItem> PlayerInventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> _itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void Start()
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

            OnInventoryUpdate?.Invoke(PlayerInventory);
            OnInventoryInit?.Invoke();
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

            // TODO: UI ������Ʈ Invoke
        }
    }
}
