using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> PlayerInventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> _itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void Awake()
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
        }
    }

    public void Add(ItemData itemData)
    {
        if(_itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToInventory();
            // TODO: �κ��丮 UI ������Ʈ Invoke?
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
            
            // TODO: UI ������Ʈ Invoke
        }
    }
}
