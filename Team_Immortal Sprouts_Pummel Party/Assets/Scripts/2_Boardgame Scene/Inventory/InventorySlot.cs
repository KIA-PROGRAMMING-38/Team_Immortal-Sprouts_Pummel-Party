using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public enum SlotState
    {
        None,
        Selected,
        UnSelected,
    }

    public SlotState CurrentSlotState = SlotState.None;
    public Image Icon;
    public TextMeshProUGUI Number;
    public ParticleSystem SelectedParticle;

    private InventoryManager _inventoryManager;
    private InventoryItem _item;
    public InventoryItem Item { get { return _item; } }

    private Color _defaultColor = Color.white;
    private Color _notHoldingColor = new Color(0.4f, 0.4f, 0.4f, 0.7f);

    private void Awake()
    {
        ChangeState(SlotState.UnSelected);
    }
    private void OnEnable()
    {
        SelectedParticle.gameObject.SetActive(false);
    }

    public void ChangeState(SlotState state)
    {
        if (CurrentSlotState == state) return;
        switch(state)
        {
            case SlotState.Selected:
                SelectedParticle.gameObject.SetActive(true);
                break;
            case SlotState.UnSelected:
                SelectedParticle.gameObject.SetActive(false);
                break;
        }

        CurrentSlotState = state;
    }

    public void DrawSlot(InventoryItem item)
    {
        if(item == null)
        {
            return;
        }

        Number.text = item.Number.ToString();

        if(item.IsHolding == false)
        {
            Icon.color = _notHoldingColor;
        }
        else
        {
            Icon.color = _defaultColor;
        }
    }

    public void SetSlotItem(InventoryItem item, InventoryManager manager)
    {
        _item = item;
        Icon.sprite = _item.ItemData.Icon;

        _inventoryManager = manager;
    }

    public void OnClick_ItemSlot()
    {
        if(_item.Number == 0)
        {
            return;
        }

        _inventoryManager.SetSelectedSlot(this);
    }
}
