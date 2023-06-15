using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventorySlot> InventorySlots = new List<InventorySlot>(8);
    
    [SerializeField] private GameObject _slotPrefab;

    private void Awake()
    {
        CreateSlots();
    }

    private void CreateSlots()
    {
        for(int i = 0; i < InventorySlots.Capacity; ++i)
        {
            GameObject newSlot = Instantiate(_slotPrefab);
            newSlot.transform.SetParent(transform, false);

            InventorySlots.Add(newSlot.GetComponent<InventorySlot>());
        }
    }

    public void InitInventorySlots(List<InventoryItem> inventory)
    {
        for (int i = 0; i < InventorySlots.Capacity; ++i)
        {
            InventorySlots[i].SetSlotItem(inventory[i], this);
        }

        CloseInventory();
    }

    public void UpdateInventory(List<InventoryItem> inventory)
    {
        for(int i = 0; i < inventory.Count; ++i)
        {
            InventorySlots[i].DrawSlot(inventory[i]);
        }
    }

    public void CloseInventory()
    {
        gameObject.SetActive(false);
    }

    public void OpenInventory()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 인벤토리 On/Off 버튼을 눌렀을 때의 이벤트를 구독
    /// </summary>
    public void OnClick_InventoryButton()
    {
        if(gameObject.activeSelf)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    private ItemData _selectedItem;
    /// <summary>
    /// 인벤토리 슬롯을 터치했을 때 선택된 아이템을 저장
    /// </summary>
    public void SetSelectedItem(ItemData selectedSlotItem)
    {
        _selectedItem = selectedSlotItem;
        Debug.Log($"selected item: {_selectedItem}");
    }
}
