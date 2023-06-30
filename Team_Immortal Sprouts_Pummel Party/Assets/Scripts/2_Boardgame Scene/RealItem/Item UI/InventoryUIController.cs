using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private Canvas[] inventoryCanvas;
    [SerializeField] private GameObject virtualJoyStick;
    private ItemSlot[] itemSlots;
    public BoardPlayerController player; // 지금은 임시로 한거임 => 보드게임매니저가 나중에 해줘야함

    private const int SELECT_CANVAS = 0;
    private const int ITEMSLOT_CANVAS = 1;
    private const int CONTROL_CANVAS = 2;

    private RealInventory playerInventory;
    private int totalItemCount;
    private int[] itemCountArray;
    [SerializeField] private RealItem selectedItem;

    public UnityEvent<int[]> OnEnableItemSlot = new UnityEvent<int[]>();

    private void Awake()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>();
    }

    private void OnEnable()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            slot.OnTouchItemSlot.RemoveListener(stopOtherSlots);
            slot.OnTouchItemSlot.AddListener(stopOtherSlots);
            slot.OnTouchItemSlot.RemoveListener(setSelectedItem);
            slot.OnTouchItemSlot.AddListener(setSelectedItem);
        }
    }

    private void Start()
    {
        playerInventory = player.GetPlayerInventory();
        totalItemCount = playerInventory.GetTotalItemCount();
        itemCountArray = new int[totalItemCount];
    }

    private void OnDisable()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            slot.OnTouchItemSlot.RemoveListener(stopOtherSlots);
            slot.OnTouchItemSlot.RemoveListener(setSelectedItem);
        }
    }

    
    #region OnClick 이벤트 함수
    /// <summary>
    /// 아이템 버튼을 눌렀을 때 아이템슬롯 캔버스를 조정하는 함수
    /// </summary>
    public void ManipulateItemSlotCanvas()
    {
        inventoryCanvas[ITEMSLOT_CANVAS].enabled = !inventoryCanvas[ITEMSLOT_CANVAS].enabled; // 껏다 켜주기
        OnEnableItemSlot?.Invoke(getItemCounts());
    }

    private bool isUsingItem = false;
    /// <summary>
    /// Select 또는 X 버튼을 눌렀을 때, 아이템 컨트롤 캔버스를 조정하는 함수
    /// </summary>
    public void ManipulateControlCanvas()
    {
        isUsingItem = !isUsingItem;

        if (selectedItem != null) // 선택한 아이템이 존재한다면
            enableControlCanvas(isUsingItem);

        if (isUsingItem)
        {
            player.ChangeToDesiredState(BoardgamePlayerAnimID.ITEM);
        }
        else
        {
            player.ChangeToDesiredState(BoardgamePlayerAnimID.HOVERING);
        }
    }
    #endregion



    private void enableControlCanvas(bool shouldTurnOn)
    {
        if (selectedItem is IControllable)
        {
            virtualJoyStick.SetActive(true);
        }
        else
        {
            virtualJoyStick.SetActive(false);
        }

        inventoryCanvas[SELECT_CANVAS].enabled = !shouldTurnOn;
        inventoryCanvas[ITEMSLOT_CANVAS].enabled = !shouldTurnOn;
        inventoryCanvas[CONTROL_CANVAS].enabled = shouldTurnOn;
    }

    private int[] getItemCounts()
    {
        for (int i = 0; i < totalItemCount; ++i) // 아이템 개수는 3개임
        {
            itemCountArray[i] = playerInventory.GetItemCount(i);
        }

        return itemCountArray;  
    }

    private void stopOtherSlots(int itemID)
    {
        for (int slotNumber = 0; slotNumber < itemSlots.Length; ++slotNumber)
        {
            if (itemID != slotNumber)
            {
                itemSlots[slotNumber].StopSelectedUI();
            }
        }
    }

    private void setSelectedItem(int itemID)
    {
        if (itemID < 0) // 음수라면 null로 만들어준다
            selectedItem = null;
        else
            selectedItem = playerInventory.GetItem(itemID);
    }
}
