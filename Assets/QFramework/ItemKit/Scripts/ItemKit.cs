using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class ItemKit : MonoBehaviour
    {

        public static void LoadItemConfigByResources(string itemName)
        {
            AddItemConfig(Resources.Load<ItemConfig>(itemName));
        }

        public static void AddItemConfig(IItem itemConfig)
        {
            ItemByKey.Add(itemConfig.GetKey,itemConfig);
        }


        public static void CreateSlot(IItem item,int count)
        {
            Slots.Add(new Slot(item,count));
        }
        
        public static List<Slot> Slots = new List<Slot>();

        public static Dictionary<string, IItem> ItemByKey = new Dictionary<string, IItem>();
       
        
        
        public static Slot FindSlotByKey(string itemKey)
        {
            return ItemKit.Slots.Find(s => s.Item != null && s.Item.GetKey == itemKey && s.Count != 0);
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

