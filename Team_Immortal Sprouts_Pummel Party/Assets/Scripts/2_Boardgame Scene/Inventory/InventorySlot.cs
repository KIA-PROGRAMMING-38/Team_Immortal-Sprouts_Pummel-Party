using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Number;

    private Color _defaultColor = Color.white;
    private Color _notHoldingColor = new Color(0.4f, 0.4f, 0.4f, 0.7f);

    public void DrawSlot(InventoryItem item)
    {
        if(item == null)
        {
            return;
        }

        Icon.sprite = item.ItemData.Icon;
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
}
