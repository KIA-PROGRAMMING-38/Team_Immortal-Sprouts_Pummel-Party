using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private Canvas[] inventoryCanvas;
    [SerializeField] private GameObject virtualJoyStick;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button selectButton;
    private ItemSlot[] itemSlots;
    public BoardPlayerController player; // 지금은 임시로 한거임 => 보드게임매니저가 나중에 해줘야함

    private const int SELECT_CANVAS = 0;
    private const int ITEMSLOT_CANVAS = 1;
    private const int CONTROL_CANVAS = 2;
    private const int DESCRIPTION_CANVAS = 3;

    private RealInventory playerInventory;
    private int totalItemCount;
    private int[] itemCountArray;
    [SerializeField] private RealItem selectedItem; // 보기 위해서 SerializeField 

    public UnityEvent<int[]> OnEnableItemSlot = new UnityEvent<int[]>();


    // 턴이 시작되었을 때, 아이템 선택 캔버스가 켜져야함 => 프레임워크가 필요함

    private void Awake()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>();
    }

    private void OnEnable()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            slot.OnTouchItemSlot.RemoveListener(stopOtherSlots);
            slot.OnTouchItemSlot.RemoveListener(setSelectedItem);
            slot.OnTouchItemSlot.RemoveListener(showItemDescription);
            slot.OnTouchItemSlot.RemoveListener(enableSelectButton);
            slot.OnTouchItemSlot.AddListener(stopOtherSlots);
            slot.OnTouchItemSlot.AddListener(setSelectedItem);
            slot.OnTouchItemSlot.AddListener(showItemDescription);
            slot.OnTouchItemSlot.AddListener(enableSelectButton);


            WaitForDictionaryInitialization().Forget();
        }
    }

    private async UniTaskVoid WaitForDictionaryInitialization()
    {
        await UniTask.Delay(2000);
        player.GetDesiredState<MoveInProgressState>(BoardgamePlayerAnimID.MOVEINPROGRESS).OnPlayerMove.RemoveListener(enableSelectCanvas);
        player.GetDesiredState<MoveInProgressState>(BoardgamePlayerAnimID.MOVEINPROGRESS).OnPlayerMove.AddListener(enableSelectCanvas);
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
            slot.OnTouchItemSlot.RemoveListener(showItemDescription);
            slot.OnTouchItemSlot.RemoveListener(enableSelectButton);

            player.GetDesiredState<MoveInProgressState>(BoardgamePlayerAnimID.MOVEINPROGRESS).OnPlayerMove.RemoveListener(enableSelectCanvas);
        }
    }


    #region OnClick 이벤트 함수
    /// <summary>
    /// 아이템 버튼을 눌렀을 때 아이템슬롯과 설명 캔버스를 조정하는 함수
    /// </summary>
    private string emptyText = string.Empty;
    public void ManipulateItemSlotCanvas()
    {
        inventoryCanvas[ITEMSLOT_CANVAS].enabled = !inventoryCanvas[ITEMSLOT_CANVAS].enabled; // 껏다 켜주기
        inventoryCanvas[DESCRIPTION_CANVAS].enabled = !inventoryCanvas[DESCRIPTION_CANVAS].enabled;
        if (!inventoryCanvas[DESCRIPTION_CANVAS].enabled)
        {
            descriptionText.text = emptyText;
            selectedItem = null;
            foreach (ItemSlot slot in itemSlots)
            {
                slot.CancleSelectItem();
            }
        }
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
        {
            enableControlCanvas(isUsingItem);
        }

        if (isUsingItem)
        {
            player.ChangeToDesiredState(BoardgamePlayerAnimID.ITEM);
            inventoryCanvas[DESCRIPTION_CANVAS].enabled = false;
        }
        else
        {
            player.ChangeToDesiredState(BoardgamePlayerAnimID.HOVERING);
            inventoryCanvas[DESCRIPTION_CANVAS].enabled = true;
        }
    }

    public void UseItem()
    {
        selectedItem.Use(); // 선택한 아이템을 사용한다
        playerInventory.DecreaseItemCount(selectedItem.GetId());
        inventoryCanvas[SELECT_CANVAS].enabled = false;
    }
    #endregion

    private void enableSelectCanvas(bool shouldTurnOn) => inventoryCanvas[SELECT_CANVAS].enabled = shouldTurnOn;

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
                itemSlots[slotNumber].CancleSelectItem();
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

    private void showItemDescription(int itemID)
    {
        if (selectedItem != null)
        {
            inventoryCanvas[DESCRIPTION_CANVAS].enabled = true;
            descriptionText.text = selectedItem.GetDescription();
        }
        else if (itemID < 0)
        {
            descriptionText.text = emptyText;
        }
    }

    private void enableSelectButton(int itemID)
    {
        if (itemID < 0)
        {
            selectButton.interactable = false;
        }
        else
        {
            selectButton.interactable = true;
        }
    }
}
