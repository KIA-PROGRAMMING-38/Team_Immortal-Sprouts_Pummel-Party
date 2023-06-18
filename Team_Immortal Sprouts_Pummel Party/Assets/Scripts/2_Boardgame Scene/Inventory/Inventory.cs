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

    public List<InventoryItem> PlayerInventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    /// <summary>
    /// 인벤토리에 초기 데이터 저장
    /// </summary>
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
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
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
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromInventory();
            OnInventoryUpdate?.Invoke();
        }
    }
}
