// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// [CreateAssetMenu(fileName = "New EquipmentData", menuName = "Inventory/EquipmentData")]
// public class EquipmentData_SO : InventoryData_SO
// {
//     public void EnsureSize(int size)
//     {
//         if (items == null) items = new List<InventoryItem>();
//         while (items.Count < size)
//         {
//             items.Add(new InventoryItem { itemData = null, amount = 0 });
//         }
//     }
//
//     public static bool IndexMatchesItemType(int index, ItemType itemType, ItemData_SO itemData = null)
//     {
//         if (index == 0 || index == 1 || index == 2 || index == 5 || index == 6 || index == 7)
//             return itemType == ItemType.Weapon;
//
//         if (index == 10) return itemType == ItemType.Head;
//         if (index == 11) return itemType == ItemType.Chest || itemType == ItemType.Armor;
//         if (index == 12) return itemType == ItemType.Hands;
//         if (index == 13) return itemType == ItemType.Legs;
//         if (index == 15 || index == 16 || index == 17) return itemType == ItemType.Accessories;
//
//         if (index >= 20 && index <= 29)
//         {
//             if (itemType == ItemType.Others) return true;
//             if (itemData != null && itemData.isConsumable) return true;
//             return false;
//         }
//         return false;
//     }
// }
