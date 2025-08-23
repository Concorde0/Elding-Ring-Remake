using System;
using System.Collections.Generic;

namespace RPG.UI
{

    public class InventoryModel
    {
        public int CurrentCategory { get; private set; }
        public event Action<int> OnCategoryChanged;
        public ItemData Item { get; private set; }

        // 内部存储每个分类下的 IItemSlotData 列表
        private readonly List<List<IItemSlotData>> categorizedItems;

        public InventoryModel()
        {
            categorizedItems = new List<List<IItemSlotData>>();
            for (int i = 0; i < 4; i++)
            {
                categorizedItems.Add(new List<IItemSlotData>());
            }
                
        }
        public void SetItem(ItemData item)
        {
            Item = item;
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
                return new List<IItemSlotData>();

            return categorizedItems[index];
        }
        
        public void SetItemsForCategory(int index, List<IItemSlotData> items)
        {
            if (index < 0 || index >= categorizedItems.Count) return;

            categorizedItems[index] = items ?? new List<IItemSlotData>();
            // 如果设置的就是当前激活分类，立刻发一次事件刷新 View
            if (index == CurrentCategory)
                OnCategoryChanged?.Invoke(index);
        }
    }
}
