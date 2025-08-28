using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryRouter
{
    [Header("分类数据")]
    public InventoryData_SO weaponData;
    public InventoryData_SO armorData;
    public InventoryData_SO accessoriesData;
    public InventoryData_SO othersData;

    private Dictionary<ItemType, InventoryData_SO> typeToData;

    public void Init()
    {
        typeToData = new Dictionary<ItemType, InventoryData_SO>
        {
            { ItemType.Weapon,      weaponData },
            { ItemType.Armor,       armorData },
            
            { ItemType.Head,        armorData },
            { ItemType.Chest,       armorData },
            { ItemType.Hands,       armorData },
            { ItemType.Legs,        armorData },

            { ItemType.Accessories, accessoriesData },
            { ItemType.Others,      othersData }
        };
    }

    public InventoryData_SO GetDataForItem(ItemData_SO item)
    {
        if (item == null) return null;
        return typeToData.TryGetValue(item.itemType, out var data) ? data : null;
    }
}

