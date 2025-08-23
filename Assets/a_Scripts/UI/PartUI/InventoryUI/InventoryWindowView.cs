using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    
    public class InventoryWindowView : UIBaseView
    {
        [Header("Category Tabs & Panels")]
        [SerializeField] private List<CategoryTabView> categoryTabs;
        [SerializeField] private List<GameObject> categoryPanels;

        [Header("Slot Prefab")]
        [SerializeField] private GameObject slotPrefab;

        [Header("Item Preview Binders")]
        [SerializeField] private InventorySumBinder   sumBinder;
        [SerializeField] private InventoryCountBinder countBinder;
        [SerializeField] private InventoryDetailBinder detailBinder;

        [Header("SelectUI & Canvas")]
        [SerializeField] private SelectUIView selectPrefab;
        [SerializeField] private Canvas        parentCanvas;

        private InventoryWindowViewModel VM => ViewModel as InventoryWindowViewModel;
        private bool _eventsBound;

        private void OnDestroy()
        {
            UnbindEvents();
        }

        protected override void OnInitialized()
        {
            for (int i = 0; i < categoryTabs.Count; i++)
            {
                int idx = i;
                var click = categoryTabs[i].GetComponent<HoverClickable>();
                click.OnLeftClick.AddListener(() =>
                {
                    VM.SwitchCategory(idx);
                });
            }
        }

        protected override void BindEvents()
        {
            if (_eventsBound || VM == null) return;
            VM.OnCategoryChanged += HandleCategoryChanged;
            _eventsBound = true;
        }

        protected override void UnbindEvents()
        {
            if (!_eventsBound || VM == null) return;
            VM.OnCategoryChanged -= HandleCategoryChanged;
            _eventsBound = false;
        }

        protected override void OnShow()
        {
            HandleCategoryChanged(VM.CurrentCategory);
        }

        private void HandleCategoryChanged(int newIndex)
        {
            for (int i = 0; i < categoryPanels.Count; i++)
                categoryPanels[i].SetActive(i == newIndex);

            PopulateCategory(newIndex);
            ClearItemPreview();
        }

        private void PopulateCategory(int categoryIndex)
        {
            var panel = categoryPanels[categoryIndex];
            var container = panel.transform.Find("SlotContainer");
            if (container == null)
            {
                Debug.LogError($"找不到 SlotContainer in {panel.name}");
                return;
            }
            
            foreach (Transform child in container)
                Destroy(child.gameObject);

            var items = VM.GetItemsByCategory(categoryIndex);
            for (int slotIndex = 0; slotIndex < items.Count; slotIndex++)
            {
                var data = items[slotIndex];
                var go   = Instantiate(slotPrefab, container, false);
                var ctrl = go.GetComponent<UISlotController>();
                
                var rect = go.GetComponent<RectTransform>();
                var ctx  = new SlotContext(slotIndex, data, Vector2.zero, rect);
                ctrl.Initialize(ctx,null);
                
                ctrl.OnHoverEnter += slotCtx =>
                {
                    if (slotCtx.ItemData is ItemData item)
                        RefreshItemPreview(item);
                };
                ctrl.OnHoverExit += _ =>
                {
                    ClearItemPreview();
                };
                
                ctrl.OnLeftClick += slotCtx =>
                {
                    var pos = (Vector2)Input.mousePosition;
                    var inst = Instantiate(selectPrefab, parentCanvas.transform);
                    inst.SetPosition(pos, parentCanvas);
                };
            }
        }

        private void RefreshItemPreview(ItemData data)
        {
            detailBinder?.SetModel(data);
            sumBinder?.SetModel(data);
            countBinder?.SetModel(data);
        }

        private void ClearItemPreview()
        {
            detailBinder?.SetModel(null);
            sumBinder?.SetModel(null);
            countBinder?.SetModel(null);
        }
    }
}