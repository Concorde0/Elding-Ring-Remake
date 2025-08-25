using System;
using System.Collections.Generic;

namespace RPG.UI
{
    public class InventoryModel
    {
        public int CurrentCategory { get; private set; }
        public event Action<int> OnCategoryChanged;
        
        private readonly List<List<IItemSlotData>> categorizedItems;
        private readonly int slotsPerCategory = 30;

        public InventoryModel()
        {
            categorizedItems = new List<List<IItemSlotData>>();
            
            for (int i = 0; i < 4; i++)
            {
                var list = new List<IItemSlotData>(slotsPerCategory);
                for (int s = 0; s < slotsPerCategory; s++)
                    list.Add(null);
                categorizedItems.Add(list);
            }
        }
        public IItemSlotData Item { get; private set; }

        public void SetItem(ItemData item)
        {
            Item = item == null ? null : new ItemSlot(item);
        }

        public void SetItem(IItemSlotData slot)
        {
            Item = slot;
        }

        public void Initialize()
        {
            SwitchCategory(0);
        }

        public void SwitchCategory(int newIndex)
        {
            if (newIndex == CurrentCategory) return;
            CurrentCategory = newIndex;
            OnCategoryChanged?.Invoke(newIndex);
        }
        
        public List<IItemSlotData> GetItemsByCategory(int index)
        {
            if (index < 0 || index >= categorizedItems.Count)
                return new List<IItemSlotData>(slotsPerCategory);
            
            return new List<IItemSlotData>(categorizedItems[index]);
        }
        
        public void SetItemsForCategory(int index, List<IItemSlotData> items)
        {
            if (index < 0 || index >= categorizedItems.Count) return;

            var newList = new List<IItemSlotData>(slotsPerCategory);
            if (items != null)
            {
                int take = Math.Min(items.Count, slotsPerCategory);
                for (int i = 0; i < take; i++)
                    newList.Add(items[i]);
            }

            while (newList.Count < slotsPerCategory)
                newList.Add(null);

            categorizedItems[index] = newList;

            if (index == CurrentCategory)
                OnCategoryChanged?.Invoke(index);
        }
        
        public bool AddItemToCategory(int index, IItemSlotData item)
        {
            if (index < 0 || index >= categorizedItems.Count || item == null)
                return false;

            var list = categorizedItems[index];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    list[i] = item;
                    if (index == CurrentCategory)
                        OnCategoryChanged?.Invoke(index);
                    return true;
                }
            }

            return false; 
        }

        public void ClearSlot(int categoryIndex, int slotIndex)
        {
            if (categoryIndex < 0 || categoryIndex >= categorizedItems.Count) return;
            if (slotIndex < 0 || slotIndex >= slotsPerCategory) return;

            categorizedItems[categoryIndex][slotIndex] = null;
            if (categoryIndex == CurrentCategory) OnCategoryChanged?.Invoke(categoryIndex);
        }
        
        public int SlotsPerCategory => slotsPerCategory;
    }
}
