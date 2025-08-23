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

        [Header("右侧详情 ViewModel 引用")] 
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

        private void HandleCategoryChanged(int newIndex)
        {
            for (int i = 0; i < categoryPanels.Count; i++)
                categoryPanels[i].SetActive(i == newIndex);

            PopulateCategory(newIndex);
        }

        private void PopulateCategory(int categoryIndex)
        {
            // 找到专门用来装槽位的容器
            var panel = categoryPanels[categoryIndex];
            var container = panel.transform.Find("SlotContainer");

            // 先清空旧槽位（只删 Container 下的子物体）
            foreach (Transform child in container)
                Destroy(child.gameObject);

            // 获取要显示的数据列表
            var items = VM.GetItemsByCategory(categoryIndex);

            // 在 Container 下面实例化新的槽位
            for (int i = 0; i < items.Count; i++)
            {
                var slotGO = Instantiate(
                    slotPrefab,
                    container,        // 父节点设为 Container
                    worldPositionStays: false
                );

                // 初始化 slot
                var controller = slotGO.GetComponent<UISlotController>();
                var rect = slotGO.GetComponent<RectTransform>();
                var ctx = new SlotContext(i, items[i], Vector2.zero, rect);
                controller.Initialize(ctx, equipmentVM);
            }
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
    }
}
