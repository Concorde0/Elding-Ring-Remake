using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;
    public TextMeshProUGUI amount = null;
    public ItemData_SO currentItemData;
    public GameObject lightBackGround;
    
    public InventoryData_SO Bag{get;set;}
    public int Index { get; set; } = -1; 

    public void SetupItemUI(ItemData_SO item, int itemAmount)
    {
        if (Bag == null || Index < 0 || Index >= Bag.items.Count)
        {
            if (icon != null) icon.gameObject.SetActive(false);
            if (amount != null) amount.text = "";
            if (lightBackGround != null) lightBackGround.SetActive(false);
            return;
        }
        
        if (itemAmount < 0) itemAmount = 0;
        if (item == null || itemAmount == 0)
        {
            currentItemData = null;
            if (icon != null) icon.gameObject.SetActive(false);
            if (amount != null) amount.text = "";
            if (lightBackGround != null) lightBackGround.SetActive(false);
            return;
        }
        
        currentItemData = item;
        if (icon != null)
        {
            icon.sprite = item.itemIcon;
            icon.gameObject.SetActive(true);
        }
        if (amount != null)
            amount.text = itemAmount.ToString();
    }

    public ItemData_SO GetItem()
    {
        return Bag.items[Index].itemData;
    }
    
}
