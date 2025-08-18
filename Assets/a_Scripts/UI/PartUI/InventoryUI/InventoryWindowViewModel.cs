using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class InventoryWindowViewModel : UIBaseViewModel
    {
        // 当前激活的分类索引
        public int CurrentCategory { get; private set; }

        // 分类切换事件，View 订阅后更新显示
        public event Action<int> OnCategoryChanged;

        public override void Initialize()
        {
            // 默认激活第 0 个分类
            SwitchCategory(0);
        }

        // 外部调用：切换到指定分类
        public void SwitchCategory(int newIndex)
        {
            if (newIndex == CurrentCategory) return;
            CurrentCategory = newIndex;
            OnCategoryChanged?.Invoke(newIndex);
        }
    }
}

