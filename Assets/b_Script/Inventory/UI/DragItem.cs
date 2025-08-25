using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private ItemUI currentItemUI;
    private SlotHolder currentHolder;
    private SlotHolder targetHolder;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform,true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
{
    GameObject pointerObj = eventData.pointerEnter;
    
    if (pointerObj != null)
    {
        targetHolder = pointerObj.GetComponent<SlotHolder>() ?? pointerObj.GetComponentInParent<SlotHolder>();
    }

    if (targetHolder != null)
    {
        switch (targetHolder.slotType)
        {
            case SlotType.BAG:
                SwapItem();
                Debug.Log("Bag");
                break;
            case SlotType.ARMOR:
                if (currentItemUI.Bag.items[currentItemUI.Index]?.itemData?.itemType == ItemType.Armor)
                {
                    SwapItem();
                    Debug.Log("ARMOR");
                }
                break;
            case SlotType.WEPON:
                if (currentItemUI.Bag.items[currentItemUI.Index]?.itemData?.itemType == ItemType.Weapon)
                {
                    SwapItem();
                    Debug.Log("Weapon");
                }
                break;
            case SlotType.ACTION:
                if (currentItemUI.Bag.items[currentItemUI.Index]?.itemData?.itemType == ItemType.Usable)
                {
                    SwapItem();
                    Debug.Log("Action");
                }
                break;
        }
        currentHolder.UpdateItem();
        targetHolder.UpdateItem();
    }
    else
    {
        Debug.LogWarning("targetHolder 为空，无法交换物品！");
    }

    transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
    
    RectTransform t = transform as RectTransform;
    t.offsetMax = -Vector2.one * 5;
    t.offsetMin = Vector2.one * 5;
}

    private void SwapItem()
    {
        // 获取当前拖拽的物品和目标格子的物品
        InventoryItem sourceItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];
        InventoryItem targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];

        // **情况 1：目标格子是空的，直接移动**
        if (targetItem == null || targetItem.itemData == null)
        {
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = new InventoryItem
            {
                itemData = sourceItem.itemData,
                amount = sourceItem.amount
            };

            // 清空原始位置的物品
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = new InventoryItem();
        }
        // **情况 2：同种可堆叠物品，合并数量**
        else if (sourceItem.itemData == targetItem.itemData && targetItem.itemData.stackable)
        {
            targetItem.amount += sourceItem.amount;

            // 清空原始位置的物品
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = new InventoryItem();
        }
        // **情况 3：普通交换**
        else
        {
            // 直接交换对象的引用，而不是创建新对象
            (currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index], targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index]) 
                = (targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index], currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index]);
        }

        // 更新 UI
        currentHolder.UpdateItem();
        targetHolder.UpdateItem();
    }



}
