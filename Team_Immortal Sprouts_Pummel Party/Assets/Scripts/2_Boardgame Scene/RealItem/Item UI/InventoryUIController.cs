using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private Canvas[] inventoryCanvas;

    public BoardPlayerController player; // 지금은 임시로 한거임 => 보드게임매니저가 나중에 해줘야함

    private const int SELECT_CANVAS = 0;
    private const int ITEMSLOT_CANVAS = 1;
    private const int CONTROL_CANVAS = 2;

    private int totalItemCount;
    private int[] itemCountArray;

    public UnityEvent<int[]> OnEnableItemSlot = new UnityEvent<int[]>();

    private void Start()
    {
        totalItemCount = player.GetPlayerInventory().GetTotalItemCount();
        itemCountArray = new int[totalItemCount];
    }

    #region OnClick 이벤트 함수
    /// <summary>
    /// 아이템 버튼을 눌렀을 때 발동하는 함수
    /// </summary>
    public void EnableItemSlotCanvas()
    {
        inventoryCanvas[ITEMSLOT_CANVAS].enabled = !inventoryCanvas[ITEMSLOT_CANVAS].enabled; // 껏다 켜주기
        OnEnableItemSlot?.Invoke(getItemCounts());
    }

    public void TouchItemSlot()
    {

    }
    #endregion

    
    private int[] getItemCounts()
    {
        for (int i = 0; i < totalItemCount; ++i) // 아이템 개수는 3개임
        {
            itemCountArray[i] = player.GetPlayerInventory().GetItemCount(i);
        }

        return itemCountArray;  
    }
}
