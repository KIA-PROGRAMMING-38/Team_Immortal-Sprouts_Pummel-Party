using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private List<InventorySlot> InventorySlots = new List<InventorySlot>(8);
    
    [SerializeField] private GameObject slotPrefab;

    //private BoardgamePlayer[] players;
    private void Awake()
    {
        CreateSlots();

        // TODO: 게임매니저 연결 시 전체 플레이어 정보 참조, 현재 클라이언트의 플레이어가 누구인지 따로 저장해야 함 (현재 테스트로 currentPlayer 사용)
        //players = FindObjectsOfType<BoardgamePlayer>();
    }

    private void OnEnable()
    {
        //foreach (BoardgamePlayer player in players)
        //{
        //    // TODO: 포톤 연결 시 isMine으로 확인
        //    if (player != currentPlayer) continue;

        //    player.Inventory.OnInventoryInit.RemoveAllListeners();
        //    player.Inventory.OnInventoryUpdate.RemoveAllListeners();
        //    player.Inventory.OnInventoryInit.AddListener(InitInventorySlots);
        //    player.Inventory.OnInventoryUpdate.AddListener(UpdateInventory);
        //}
    }

    private void OnDestroy()
    {
        //foreach (BoardgamePlayer player in players)
        //{
        //    // TODO: 포톤 연결 시 isMine으로 확인
        //    if (player != currentPlayer) continue;

        //    player.Inventory.OnInventoryInit.RemoveAllListeners();
        //    player.Inventory.OnInventoryUpdate.RemoveAllListeners();
        //}
    }

    private void CreateSlots()
    {
        for(int i = 0; i < InventorySlots.Capacity; ++i)
        {
            GameObject newSlot = Instantiate(slotPrefab);
            newSlot.transform.SetParent(transform, false);

            InventorySlots.Add(newSlot.GetComponent<InventorySlot>());
        }
    }

    private List<BelongingItemData> playerInventory;
    /// <summary>
    /// 관리할 인벤토리 및 슬롯 정보가 초기화된 후 인벤토리 UI를 비활성화
    /// </summary>
    public void InitInventorySlots(Inventory inventory)
    {
        playerInventory = inventory.PlayerInventory;
        for (int i = 0; i < InventorySlots.Capacity; ++i)
        {
            InventorySlots[i].SetSlotItem(playerInventory[i], this);
        }

        CloseInventory();
    }

    /// <summary>
    /// 업데이트된 인벤토리 정보를 반영
    /// </summary>
    public void UpdateInventory()
    {
        for (int i = 0; i < playerInventory.Count; ++i)
        {
            InventorySlots[i].DrawSlot(playerInventory[i]);
        }
    }

    [SerializeField] private Image backgroundPanel;
    /// <summary>
    /// 인벤토리를 닫음 (UI 비활성화, 선택된 아이템 초기화)
    /// </summary>
    public void CloseInventory()
    {
        selectedSlot?.ChangeState(InventorySlot.SlotState.UnSelected);
        setSelectedData(null);
        itemSelectButton.interactable = false;

        gameObject.SetActive(false);
        backgroundPanel.gameObject.SetActive(false);
    }

    private void OpenInventory()
    {
        gameObject.SetActive(true);
        backgroundPanel.gameObject.SetActive(true);
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

    private InventorySlot selectedSlot;
    private ItemData selectedItem;
    [SerializeField] private Button itemSelectButton;

    private void setSelectedData(InventorySlot slot)
    {
        selectedSlot = slot;
        selectedItem = slot != null ? slot.Item.ItemData : null;
    }

    /// <summary>
    /// 인벤토리 슬롯을 터치했을 때 선택된 슬롯 및 아이템을 저장
    /// </summary>
    public void SetSelectedSlot(InventorySlot slot)
    {
        if(selectedSlot == null)
        {
            setSelectedData(slot);
            selectedSlot.ChangeState(InventorySlot.SlotState.Selected);
            itemSelectButton.interactable = true;
        }
        else if (slot == selectedSlot)
        {
            selectedSlot.ChangeState(InventorySlot.SlotState.UnSelected);
            setSelectedData(null);

            itemSelectButton.interactable = false;
        }
        else
        {
            selectedSlot.ChangeState(InventorySlot.SlotState.UnSelected);
            setSelectedData(slot);
            selectedSlot.ChangeState(InventorySlot.SlotState.Selected);
            itemSelectButton.interactable = true;
        }
    }

    //public BoardgamePlayer currentPlayer;
    [SerializeField] private ItemControllGroup itemControllGroup;
    /// <summary>
    /// 사용할 아이템을 선택하는 버튼 클릭 이벤트로 호출
    /// </summary>
    public void SelectItem()
    {
        //IUsable item = Instantiate(selectedItem.Prefab).GetComponent<IUsable>();
        //item.SetForUse(currentPlayer);

        //currentPlayer.Inventory.Remove(selectedItem);

        //if(selectedItem.isControllable)
        //{
        //    itemControllGroup.gameObject.SetActive(true);
        //}

        selectedSlot.ChangeState(InventorySlot.SlotState.UnSelected);
        setSelectedData(null);
    }
}
