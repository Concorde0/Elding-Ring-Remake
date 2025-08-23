using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class InventoryWindowView : UIBaseView
    {
        [Header("四个分类按钮")]
        [SerializeField] private List<CategoryTabView> categoryTabs;

        [Header("四个分类面板")]
        [SerializeField] private List<GameObject> categoryPanels;

        [Header("References")]
        [SerializeField] private Canvas parentCanvas;
        [SerializeField] private SelectUIView selectPrefab;

        [Header("Prefabs")]
        [SerializeField] private GameObject slotPrefab;

        // 这里我们多拿一个 PlayerStatsWindowViewModel 的引用，用来联动右侧装备详情面板
        [SerializeField] private PlayerStatsWindowViewModel equipmentVM;

        private InventoryWindowViewModel VM => ViewModel as InventoryWindowViewModel;

        protected override void BindEvents()
        {
            if (VM != null)
                VM.OnCategoryChanged += HandleCategoryChanged;

            SelectUIEvent();
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
        /// 根据分类索引切换显示的面板
        /// </summary>
        private void HandleCategoryChanged(int newIndex)
        {
            for (int i = 0; i < categoryPanels.Count; i++)
                categoryPanels[i].SetActive(i == newIndex);

            PopulateCategory(newIndex);
        }

        private void SelectUIEvent()
        {
            var clickables = GetComponentsInChildren<HoverClickable>(true);
            foreach (var clickable in clickables)
            {
                clickable.OnLeftClick.RemoveAllListeners();
                clickable.OnLeftClick.AddListener(() =>
                {
                    Vector2 mousePos = Input.mousePosition;
                    var instance = Instantiate(selectPrefab, parentCanvas.transform);
                    instance.SetPosition(mousePos, parentCanvas);
                });
            }
        }

        /// <summary>
        /// 动态生成分类下的物品槽位
        /// </summary>
        private void PopulateCategory(int categoryIndex)
        {
            var items = VM.GetItemsByCategory(categoryIndex);

            // 清空旧槽位
            foreach (Transform child in categoryPanels[categoryIndex].transform)
                Destroy(child.gameObject);

            for (int i = 0; i < items.Count; i++)
            {
                var slotGO = Instantiate(slotPrefab, categoryPanels[categoryIndex].transform);
                var controller = slotGO.GetComponent<UISlotController>();
                var rect = slotGO.GetComponent<RectTransform>();
                var item = items[i];

                // 新 SlotContext 直接用 IItemSlotData，不重复存储名称/ID/Icon
                var ctx = new SlotContext(
                    i,
                    item,
                    Vector2.zero,
                    rect
                );

                // 初始化时直接把装备 VM 传给槽位，这样悬停自动刷新右侧面板
                controller.Initialize(ctx, equipmentVM);

                controller.OnLeftClick += HandleLeftClick;
                controller.OnRightClick += HandleRightClick;
            }
        }

        private void HandleLeftClick(SlotContext ctx)
        {
            Debug.Log($"左键点击槽位 {ctx.SlotIndex}");
        }

        private void HandleRightClick(SlotContext ctx)
        {
            Debug.Log($"右键点击槽位 {ctx.SlotIndex}");
        }
    }
}
