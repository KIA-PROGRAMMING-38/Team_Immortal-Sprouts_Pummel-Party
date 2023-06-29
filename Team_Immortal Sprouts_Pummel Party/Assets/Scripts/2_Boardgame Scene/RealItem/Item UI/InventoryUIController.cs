using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private Canvas[] inventoryCanvas;
    [SerializeField] private ItemSlot[] itemButtons;

    public BoardPlayerController player; // 지금은 임시로 한거임 => 보드게임매니저가 나중에 해줘야함

    private const int SELECT_CANVAS = 0;
    private const int ITEMSLOT_CANVAS = 1;
    private const int CONTROL_CANVAS = 2;

    private int totalItemCount;

    private void Awake()
    {
        itemButtons = GetComponentsInChildren<ItemSlot>();
    }

    private void Start()
    {
        totalItemCount = player.GetPlayerInventory().GetTotalItemCount();
    }

    public void EnableItemSlotCanvas()
    {
        inventoryCanvas[ITEMSLOT_CANVAS].enabled = !inventoryCanvas[ITEMSLOT_CANVAS].enabled; // 껏다 켜주기
        EnableItemSlot();
    }

    public void TouchItemSlot()
    {

    }


    private void EnableItemSlot()
    {
        for (int i = 0; i < totalItemCount; ++i) // 아이템 개수는 3개임
        {
            if (1 <= player.GetPlayerInventory().GetItemCount(i)) // 아이템이 1개 이상이면
            {
                //itemButtons[i].interactable = true;
            }
            else // 아니면
            {
                //itemButtons[i].interactable = false;
            }
        }
    }
}
