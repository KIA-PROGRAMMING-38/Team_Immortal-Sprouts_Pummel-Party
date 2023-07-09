using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControllGroup : MonoBehaviour
{
    [SerializeField] private GameObject inventoryCanvas;
    private void OnEnable()
    {
        inventoryCanvas.SetActive(false);
    }

    /// <summary>
    /// 아이템 사용 버튼을 눌렀을 때 호출
    /// </summary>
    public void OnClick_ItemUseButton()
    {
        inventoryCanvas.SetActive(true);
        //inventoryCanvas.GetComponentInChildren<InventoryManager>().CloseInventory();
        gameObject.SetActive(false);
    }
}
