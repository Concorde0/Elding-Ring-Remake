using UnityEngine;

namespace RPG.UI
{
    public class CategoryTabView : MonoBehaviour
    {
        private InventoryWindowViewModel vm;
        private int index;

        public void Initialize(InventoryWindowViewModel viewModel, int tabIndex, HoverClickable hoverClickable)
        {
            vm = viewModel;
            index = tabIndex;

            // 绑定左键点击事件
            if (hoverClickable != null)
            {
                hoverClickable.OnLeftClick.RemoveAllListeners(); // 避免重复绑定
                hoverClickable.OnLeftClick.AddListener(() => vm?.SwitchCategory(index));
            }
        }
    }
}