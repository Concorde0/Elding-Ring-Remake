// InventoryWindowViewModel.cs
using System;
using System.Collections.Generic;

namespace RPG.UI
{
    public class InventoryWindowViewModel : UIBaseViewModel
    {
        /// <summary>
        /// 不带参数的构造，内部 new 一个默认 Model
        /// 方便 UIManager 通过 new TViewModel() 使用
        /// </summary>
        public InventoryWindowViewModel() : this(new InventoryModel()) { }

        /// <summary>
        /// 支持注入自定义 Model
        /// </summary>
        public InventoryWindowViewModel(InventoryModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            // 当 Model 切换分类时，把事件向上传递给 View
            _model.OnCategoryChanged += idx => OnCategoryChanged?.Invoke(idx);
        }

        private readonly InventoryModel _model;

        /// <summary>
        /// 当前激活的分类索引
        /// </summary>
        public int CurrentCategory => _model.CurrentCategory;

        /// <summary>
        /// 分类切换事件
        /// </summary>
        public event Action<int> OnCategoryChanged;

        public override void Initialize()
        {
            // 初始化 Model（会发一次 OnCategoryChanged(0)）
            _model.Initialize();
        }

        /// <summary>
        /// 让外部调用切换分类（例如 Tab 点击）
        /// </summary>
        public void SwitchCategory(int newIndex)
        {
            _model.SwitchCategory(newIndex);
        }

        /// <summary>
        /// 供 View 调用：获取某分类下的数据
        /// </summary>
        public List<IItemSlotData> GetItemsByCategory(int categoryIndex)
        {
            return _model.GetItemsByCategory(categoryIndex);
        }

        /// <summary>
        /// 供业务层调用：设置或刷新某分类下的物品列表
        /// </summary>
        public void SetItemsForCategory(int categoryIndex, List<IItemSlotData> items)
        {
            _model.SetItemsForCategory(categoryIndex, items);
        }
    }
}