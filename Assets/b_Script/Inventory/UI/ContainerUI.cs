using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    [Header("格子引用")]
    public SlotHolder[] slotHolders;

    [Header("绑定数据源")]
    public InventoryData_SO inventoryData;
    
    public void BindData(InventoryData_SO data)
    {
        inventoryData = data;
        foreach (var s in slotHolders)
            s.itemUI.Bag = data;

        RefreshUI();
    }

   
    public void RefreshUI()
    {
        if (inventoryData == null || slotHolders == null) return;

        for (int i = 0; i < slotHolders.Length; i++)
        {
            var s = slotHolders[i];
            s.itemUI.Index = i;
            
            if (s.itemUI.Bag == null) s.itemUI.Bag = inventoryData;

            s.UpdateItem(); 
        }
    }
}