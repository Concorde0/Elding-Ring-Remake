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
            for (int i = 0; i < categoryTabs.Count; i++)
            {
                var hover = categoryTabs[i].GetComponent<HoverClickable>();
                categoryTabs[i].Initialize(VM, i, hover);
            }
        }


        protected override void OnShow()
        {
            base.OnShow();
            HandleCategoryChanged(VM.CurrentCategory);
        }

        /// <summary>
        /// 根据分类索引切换显示的面板（同时就是格子容器）
        /// </summary>
        private void HandleCategoryChanged(int newIndex)
        {
            for (int i = 0; i < categoryPanels.Count; i++)
                categoryPanels[i].SetActive(i == newIndex);
        }
    }
}