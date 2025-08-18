using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class InventoryWindowView : UIBaseView
    {
        [Header("四个分类按钮（按顺序填入）")]
        [SerializeField] private List<CategoryTabView> categoryTabs;

        [Header("四个分类面板（对应顺序填入）")]
        [SerializeField] private List<GameObject> categoryPanels;

        private InventoryWindowViewModel VM => ViewModel as InventoryWindowViewModel;

        protected override void BindEvents()
        {
            VM.OnCategoryChanged += HandleCategoryChanged;
        }

        protected override void UnbindEvents()
        {
            if (VM != null)
                VM.OnCategoryChanged -= HandleCategoryChanged;
        }

        protected override void OnInitialized()
        {
            // 注入 ViewModel 与 索引
            for (int i = 0; i < categoryTabs.Count; i++)
            {
                categoryTabs[i].Initialize(VM, i);
            }
        }

        protected override void OnShow()
        {
            base.OnShow();
            // 根据当前索引刷新一次
            HandleCategoryChanged(VM.CurrentCategory);
        }

        private void HandleCategoryChanged(int newIndex)
        {
            for (int i = 0; i < categoryPanels.Count; i++)
            {
                // 只激活当前分类对应的面板
                categoryPanels[i].SetActive(i == newIndex);
            }
        }
    }
}


