using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemData_SO newItemData, int amount)
    {
        bool found = false;

        //判断它是否可堆叠，如果可堆叠的话循环列表，看看是否有相同物品，如果有，则在原有的数量上加上捡起的数量
        if (newItemData.stackable)
        {
            foreach (var item in items)
            {
                if (item.itemData == newItemData)
                {
                    item.amount += amount;
                    found = true;
                    break;
                }
            }
        }
        //如果不可堆叠，或者没有相同的物品时，放入
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemData == null && !found)
            {
                items[i].itemData = newItemData;
                items[i].amount += amount;
                break;
            }
        }
    }
}
[System.Serializable]
public class InventoryItem
{
    
    public ItemData_SO itemData;
    public int amount;
}
