using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class InventoryWindowViewModel : UIBaseViewModel
    {
        public int CurrentCategory { get; private set; }
        
        public event Action<int> OnCategoryChanged;
        
        private readonly List<List<IItemSlotData>> categorizedItems = new();

        public override void Initialize()
        {
            for (int i = 0; i < 4; i++)
                categorizedItems.Add(new List<IItemSlotData>());

            // 默认激活第 0 个分类
            SwitchCategory(0);
        }

        /// <summary>
        /// 切换到指定分类
        /// </summary>
        public void SwitchCategory(int newIndex)
        {
            if (newIndex == CurrentCategory) return;
            CurrentCategory = newIndex;
            OnCategoryChanged?.Invoke(newIndex);
        }

        /// <summary>
        /// 获取某个分类下的物品列表
        /// </summary>
        public List<IItemSlotData> GetItemsByCategory(int categoryIndex)
        {
            if (categoryIndex < 0 || categoryIndex >= categorizedItems.Count)
                return new List<IItemSlotData>();

            return categorizedItems[categoryIndex];
        }

        /// <summary>
        /// 设置某个分类的物品列表
        /// </summary>
        public void SetItemsForCategory(int categoryIndex, List<IItemSlotData> items)
        {
            if (categoryIndex < 0 || categoryIndex >= categorizedItems.Count) return;
            categorizedItems[categoryIndex] = items ?? new List<IItemSlotData>();

            // 如果当前分类正是被显示的分类，可以主动发事件刷新 View
            if (categoryIndex == CurrentCategory)
                OnCategoryChanged?.Invoke(categoryIndex);
        }
    }
}