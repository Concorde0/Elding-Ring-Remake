using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    BAG,WEPON,ARMOR,ACTION
}
public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        //双击使用，用取余来限定点击大于两次时的处理逻辑
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem())
        {
            InventoryManager.Instance.tooltip.SetupTooltip(itemUI.GetItem());
            InventoryManager.Instance.tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
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
            if (itemUI.GetItem().itemType == ItemType.Usable && itemUI.Bag.items[itemUI.Index].amount > 0 )
            {
                GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().useableData.healthPoint);
                itemUI.Bag.items[itemUI.Index].amount -= 1;
                
                QuestManager.Instance.UpdateQuestProgress(itemUI.GetItem().itemName,-1);
            } 
            UpdateItem();
        }
       
    }
    
    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.WEPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
            
        }
        var item = itemUI.Bag.items[itemUI.Index]; 
        itemUI.SetupItemUI(item.itemData, item.amount);

        // if (itemUI.Bag == null)
        // {
        //     Debug.LogWarning("itemUI.Bag 为空，尝试重新获取 InventoryData...");
        //     itemUI.Bag = InventoryManager.Instance.inventoryData;
        // }

        

        
    }

}