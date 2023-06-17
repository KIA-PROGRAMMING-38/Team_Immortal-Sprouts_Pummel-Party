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
    /// ������ ��� ��ư�� ������ �� ȣ��
    /// </summary>
    public void OnClick_ItemUseButton()
    {
        inventoryCanvas.SetActive(true);
        inventoryCanvas.GetComponentInChildren<InventoryManager>().CloseInventory();
        gameObject.SetActive(false);
    }
}
