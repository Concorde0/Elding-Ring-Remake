using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class ItemKit : MonoBehaviour
    {
        public static Item Item1 = new Item("Item_1", "物品1");
        public static Item Item2 = new Item("Item_2", "物品2");
        public static Item Item3 = new Item("Item_3", "物品3");

        public static List<Slot> Slots = new List<Slot>()
        {
            new Slot(ItemKit.Item1,1),
            new Slot(ItemKit.Item2,10),
            new Slot(ItemKit.Item3,1),
        };

        public static Dictionary<string, Item> ItemByKey = new Dictionary<string, Item>()
        {
            {ItemKit.Item1.Key,ItemKit.Item1},
            {ItemKit.Item2.Key,ItemKit.Item2},
            {ItemKit.Item3.Key,ItemKit.Item3},
        };
        
        
        public static Slot FindSlotByKey(string itemKey)
        {
            return ItemKit.Slots.Find(s => s.Item != null && s.Item.Key == itemKey && s.Count != 0);
        }

        public static Slot FindEmptySlot()
        {
            return ItemKit.Slots.Find(s => s.Count == 0);
        }
        
        public static Slot FindAddableSlot(string itemKey)
        {
            var slot = FindSlotByKey(itemKey);
            if (slot == null)
            {
                slot = FindEmptySlot();
                if (slot != null)
                {
                    slot.Item = ItemKit.ItemByKey[itemKey];
                }
            }

            return slot;
        }
        
        public static bool AddItem(string itemKey, int addCount = 1)
        {
            var slot = FindAddableSlot(itemKey);
            if (slot == null)
            {
                return false;
            }
            slot.Count += addCount;
            return true;
        }

        public static bool SubItem(string itemKey, int subCount = 1)
        {
            var slot = FindSlotByKey(itemKey);
            if (slot != null && slot.Count >= subCount)
            {
                slot.Count -= subCount;
                return true;
            }
            return false;
        }
    }
    
   

   
}

