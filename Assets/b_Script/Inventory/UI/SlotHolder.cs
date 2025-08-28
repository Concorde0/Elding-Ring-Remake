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
            InventoryContextMenu.Instance?.Open(this, eventData.position);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var item = itemUI.GetItem();
        if (item != null)
        {
            if (itemUI.lightBackGround != null)
                itemUI.lightBackGround.SetActive(true);
            
            ToolTipUI.Instance?.Show(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemUI.lightBackGround != null)
            itemUI.lightBackGround.SetActive(false);

        ToolTipUI.Instance?.Hide();
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

        //直接把数据也清掉
        if (slotType == SlotType.Equipment)
        {
            if (entry.itemData == null || entry.itemData.allowedSlot != equipmentSlot)
            {
                bag.items[itemUI.Index].itemData = null;
                bag.items[itemUI.Index].amount = 0;
                itemUI.SetupItemUI(null, 0);
                return;
            }
        }

        itemUI.SetupItemUI(entry.itemData, entry.amount);
    }

    private bool IsItemAllowed(ItemData_SO itemData)
    {
        if (slotType != SlotType.Equipment) return true;
        return itemData.allowedSlot == equipmentSlot;
    }
}
