using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI
{
    public class CategoryTabView : MonoBehaviour
    {
        private InventoryWindowViewModel vm;
        private int index;

        /// <summary>
        /// 注入 ViewModel 与本 Tab 的索引
        /// </summary>
        public void Initialize(InventoryWindowViewModel viewModel, int tabIndex)
        {
            vm    = viewModel;
            index = tabIndex;
        }

        /// <summary>
        /// 点击时切换到对应分类
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            vm?.SwitchCategory(index);
        }
    }
}

