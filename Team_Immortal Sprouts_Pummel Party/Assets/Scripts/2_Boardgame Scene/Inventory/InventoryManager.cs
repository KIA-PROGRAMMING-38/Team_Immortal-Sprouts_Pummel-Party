using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Image BackgroundPanel;
    public List<InventorySlot> InventorySlots = new List<InventorySlot>(8);
    
    [SerializeField] private GameObject _slotPrefab;

    private void Awake()
    {
        CreateSlots();
        Inventory.OnInventoryInit.AddListener(InitInventorySlots);
        Inventory.OnInventoryUpdate.AddListener(UpdateInventory);

        BackgroundPanel.gameObject.SetActive(false);
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
        _selectedSlot?.ChangeState(InventorySlot.SlotState.UnSelected);
        setSelectedData(null);
        itemSelectButton.interactable = false;
        gameObject.SetActive(false);
        BackgroundPanel.gameObject.SetActive(false);
    }

    public void OpenInventory()
    {
        gameObject.SetActive(true);
        BackgroundPanel.gameObject.SetActive(true);
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

    private InventorySlot _selectedSlot;
    private ItemData _selectedItem;
    [SerializeField] private Button itemSelectButton;

    private void setSelectedData(InventorySlot slot)
    {
        _selectedSlot = slot;
        _selectedItem = slot != null ? slot.Item.ItemData : null;
    }

    /// <summary>
    /// 인벤토리 슬롯을 터치했을 때 선택된 아이템을 저장
    /// </summary>
    public void SetSelectedSlot(InventorySlot selectedSlot)
    {
        if(_selectedSlot == null)
        {
            setSelectedData(selectedSlot);
            _selectedSlot.ChangeState(InventorySlot.SlotState.Selected);
            itemSelectButton.interactable = true;
        }
        else if (selectedSlot == _selectedSlot)
        {
            _selectedSlot.ChangeState(InventorySlot.SlotState.UnSelected);
            setSelectedData(null);

            itemSelectButton.interactable = false;
        }
        else
        {
            _selectedSlot.ChangeState(InventorySlot.SlotState.UnSelected);
            setSelectedData(selectedSlot);
            _selectedSlot.ChangeState(InventorySlot.SlotState.Selected);
            itemSelectButton.interactable = true;
        }
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

        _selectedSlot.ChangeState(InventorySlot.SlotState.UnSelected);
        setSelectedData(null);
    }
}
