using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Number;

    public void DrawSlot(InventoryItem item)
    {
        if(item == null)
        {
            return;
        }

        Icon.sprite = item.ItemData.Icon;
        Number.text = item.Number.ToString();
    }
}
