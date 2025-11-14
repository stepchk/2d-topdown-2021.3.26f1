using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    private InventoryManager manager;

    private void Awake()
    {
        Deselect();
        manager = FindObjectOfType<InventoryManager>();
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }

    // Drag and drop
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;

            // Automatically select this slot after a drop
            if (manager != null)
            {
                manager.ChangeSelectedSlot(System.Array.IndexOf(manager.inventorySlots, this));
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.ChangeSelectedSlot(System.Array.IndexOf(manager.inventorySlots, this));
        Debug.Log("Slot clicked: " + gameObject.name);
    }
}
