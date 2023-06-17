using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<InventorySlot> InventorySlots = new List<InventorySlot>(8);
    
    [SerializeField] private GameObject _slotPrefab;

    private void Awake()
    {
        CreateSlots();
        Inventory.OnInventoryInit.AddListener(InitInventorySlots);
        Inventory.OnInventoryUpdate.AddListener(UpdateInventory);
    }

    private void OnDestroy()
    {
        Inventory.OnInventoryInit.RemoveAllListeners();
        Inventory.OnInventoryUpdate.RemoveAllListeners();
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
    /// �κ��丮 On/Off ��ư�� ������ ���� �̺�Ʈ�� ����
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
    [SerializeField] private Button itemSelectButton;
    /// <summary>
    /// �κ��丮 ������ ��ġ���� �� ���õ� �������� ����
    /// </summary>
    public void SetSelectedItem(ItemData selectedSlotItem)
    {
        if (selectedSlotItem == _selectedItem)
        {
            _selectedItem = null;
            itemSelectButton.interactable = false;
        }
        else
        {
            _selectedItem = selectedSlotItem;
            itemSelectButton.interactable = true;
        }

        Debug.Log($"selected item: {_selectedItem}");
    }

    [SerializeField] private BoardgamePlayer currentPlayer;
    [SerializeField] private ItemControllGroup itemControllGroup;
    public void SelectItem()
    {
        IUsable item = Instantiate(_selectedItem.Prefab).GetComponent<IUsable>();
        item.SetForUse(currentPlayer);

        currentPlayer.Inventory.Remove(_selectedItem);

        if(_selectedItem.isControllable)
        {
            itemControllGroup.gameObject.SetActive(true);
        }
    }
}