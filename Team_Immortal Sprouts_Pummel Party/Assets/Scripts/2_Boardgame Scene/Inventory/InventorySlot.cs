using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    /// <summary>
    /// 선택된 슬롯인지, 선택되지 않은 슬롯인지 관리하기 위한 데이터
    /// </summary>
    public enum SlotState
    {
        None,
        Selected,
        UnSelected,
    }

    public SlotState CurrentSlotState = SlotState.None;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI number;
    [SerializeField] private ParticleSystem selectedParticle;

    private InventoryManager inventoryManager;
    private BelongingItemData item;
    public BelongingItemData Item { get { return item; } }

    private Color defaultColor = Color.white;
    private Color notHoldingColor = new Color(0.4f, 0.4f, 0.4f, 0.7f);

    private void Awake()
    {
        ChangeState(SlotState.UnSelected);
    }
    private void OnEnable()
    {
        selectedParticle.gameObject.SetActive(false);
    }

    public void ChangeState(SlotState state)
    {
        if (CurrentSlotState == state) return;
        switch(state)
        {
            case SlotState.Selected:
                selectedParticle.gameObject.SetActive(true);
                break;
            case SlotState.UnSelected:
                selectedParticle.gameObject.SetActive(false);
                break;
        }

        CurrentSlotState = state;
    }

    /// <summary>
    /// 인벤토리 정보(아이템 보유 개수)가 변경될 때 호출되어 UI에 정보를 반영
    /// </summary>
    public void DrawSlot(BelongingItemData item)
    {
        if(item == null)
        {
            return;
        }

        number.text = item.Number.ToString();

        if(item.IsHolding == false)
        {
            icon.color = notHoldingColor;
        }
        else
        {
            icon.color = defaultColor;
        }
    }

    /// <summary>
    /// 슬롯에 표시될 정보를 세팅 및 Presenter와 참조 연결
    /// </summary>
    public void SetSlotItem(BelongingItemData item, InventoryManager manager)
    {
        this.item = item;
        //icon.sprite = item.ItemData.Icon;

        inventoryManager = manager;
    }

    public void OnClick_ItemSlot()
    {
        //if (inventoryManager.currentPlayer.CanUseItem == false)
        //{
        //    return;
        //}

        //if (item.Number == 0)
        //{
        //    return;
        //}

        //inventoryManager.SetSelectedSlot(this);
    }
}
