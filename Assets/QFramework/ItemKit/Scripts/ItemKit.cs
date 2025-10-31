using System.Collections;
using System.Collections.Generic;
using QFramework.Example;
using UnityEngine;

namespace QFramework
{
    public class ItemKit : MonoBehaviour
    {

        public static UISlot CurrentSlotPointerOn = null;
        
        public static Dictionary<string,SlotGroup> mSlotGruopByKey = new Dictionary<string,SlotGroup>();

        public static SlotGroup GetSlotGroupByKey(string key) => mSlotGruopByKey[key];
        public static SlotGroup CreatSlotGroup(string key)
        {
            var slotGroup = new SlotGroup()
            {
                Key = key
            };
            mSlotGruopByKey.Add(key,slotGroup);
            return slotGroup;
        }
        
        public static void LoadItemDatabase(string databaseName)
        {
            var database = Resources.Load<ItemDatabase>(databaseName);
            foreach (var databaseItem in database.Items)
            {
                AddItemConfig(databaseItem);
            }
        }

        public static void AddItemConfig(IItem itemConfig)
        {
            ItemByKey.Add(itemConfig.GetKey,itemConfig);
        }

        public static Dictionary<string, IItem> ItemByKey = new Dictionary<string, IItem>();
        
    }
    
   

   
}

