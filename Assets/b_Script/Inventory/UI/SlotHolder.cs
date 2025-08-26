using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    Bag,
    Action,
    Equipment,
    Others
}

public enum EquipmentSlot
{
    None,
    Weapon,
    Head,  
    Chest,
    Hands,
    Legs,
    Accessories
}

public class SlotHolder : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;
    public EquipmentSlot equipmentSlot;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (eventData.clickCount >= 2)
            {
                UseItem();
                return;
            }
            if (InventoryContextMenu.Instance != null)
                InventoryContextMenu.Instance.Open(this, eventData.position);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem() != null)
        {
            if (itemUI.lightBackGround != null)
                itemUI.lightBackGround.SetActive(true);

            InventoryManager.Instance.tooltip.SetupTooltip(itemUI.GetItem());
            InventoryManager.Instance.tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemUI.lightBackGround != null)
            itemUI.lightBackGround.SetActive(false);

        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }

    public void UseItem()
    {
        if (itemUI.GetItem() != null)
        {
            if (itemUI.GetItem().useAble && itemUI.Bag.items[itemUI.Index].amount > 0)
            {
                GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().useableData.healthPoint);
                itemUI.Bag.items[itemUI.Index].amount -= 1;

                QuestManager.Instance.UpdateQuestProgress(itemUI.GetItem().itemName, -1);
            }
            UpdateItem();
        }
    }

    public void UpdateItem()
    {
        if (itemUI.Bag == null)
        {
            switch (slotType)
            {
                case SlotType.Bag:       itemUI.Bag = InventoryManager.Instance.inventoryData; break;
                case SlotType.Action:    itemUI.Bag = InventoryManager.Instance.actionData;    break;
                case SlotType.Equipment: itemUI.Bag = InventoryManager.Instance.equipmentData; break;
            }
        }

        var bag = itemUI.Bag;
        if (bag == null || itemUI.Index < 0 || itemUI.Index >= bag.items.Count)
        {
            itemUI.SetupItemUI(null, 0);
            return;
        }

        var entry = bag.items[itemUI.Index];

        //装备槽过滤逻辑
        if (slotType == SlotType.Equipment && entry.itemData != null && !IsItemAllowed(entry.itemData))
        {
            itemUI.SetupItemUI(null, 0);
            return;
        }

        itemUI.SetupItemUI(entry.itemData, entry.amount);
    }

    private bool IsItemAllowed(ItemData_SO itemData)
    {
        if (slotType != SlotType.Equipment) return true;

        switch (equipmentSlot)
        {
            case EquipmentSlot.Weapon:
                return itemData.itemType == ItemType.Weapon;
            case EquipmentSlot.Head:
                return itemData.itemType == ItemType.Head;
            case EquipmentSlot.Chest:
                return itemData.itemType == ItemType.Chest;
            case EquipmentSlot.Hands:
                return itemData.itemType == ItemType.Hands;
            case EquipmentSlot.Legs:
                return itemData.itemType == ItemType.Legs;
            case EquipmentSlot.Accessories:
                return itemData.itemType == ItemType.Accessories;
            default:
                return false;
        }
    }
}
