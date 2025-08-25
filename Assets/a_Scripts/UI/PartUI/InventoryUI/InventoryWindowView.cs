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
        [SerializeField] private Canvas parentCanvas;
        
        private SelectUIView currentSelectUIView;

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
                int index = i;
                var click = categoryTabs[i].GetComponent<HoverClickable>();
                click.OnLeftClick.RemoveAllListeners();
                click.OnLeftClick.AddListener(() =>
                {
                    VM.SwitchCategory(index);
                });
            }
        }

        protected override void BindEvents()
        {
            if (_eventsBound || VM == null) return;
            VM.OnCategoryChanged += HandleCategoryChanged;
            VM.OnItemChanged -= RefreshItemUI;
            VM.OnItemChanged += RefreshItemUI;
            _eventsBound = true;
        }

        protected override void UnbindEvents()
        {
            if (!_eventsBound || VM == null) return;
            VM.OnCategoryChanged -= HandleCategoryChanged;
            VM.OnItemChanged -= RefreshItemUI;
            _eventsBound = false;
        }

        protected override void OnShow()
        {
            VM.ReassignSlotsByTemplateCategory();
            HandleCategoryChanged(VM.CurrentCategory);
        }

        private void HandleCategoryChanged(int newIndex)
        {
            for (int i = 0; i < categoryPanels.Count; i++)
                categoryPanels[i].SetActive(i == newIndex);

            PopulateCategory(newIndex);
            ClearItemUI();
        }

        private void PopulateCategory(int categoryIndex)
        {
            var panel = categoryPanels[categoryIndex];
            var container = panel.transform.Find("SlotContainer");

            foreach (Transform child in container)
                Destroy(child.gameObject);

            var items = VM.GetItemsByCategory(categoryIndex);

            for (int slotIndex = 0; slotIndex < items.Count; slotIndex++)
            {
                int sIdx = slotIndex;
                IItemSlotData data = items[sIdx]; // 可能为 null

                var go   = Instantiate(slotPrefab, container, false);
                var ctrl = go.GetComponent<UISlotController>();

                var rect = go.GetComponent<RectTransform>();
                var ctx  = new SlotContext(sIdx, data, Vector2.zero, rect);

                ctrl.Initialize(ctx, null);
                
                ctrl.OnHoverEnter += (slotCtx) =>
                {
                    VM.SelectSlot(slotCtx.ItemData);
                };

                ctrl.OnHoverExit += (slotCtx) =>
                {
                    VM.ClearSelection();
                    ClearItemUI();
                };

                ctrl.OnLeftClick += (slotCtx) =>
                {
                    VM.SelectSlot(slotCtx.ItemData);
                    Vector2 pos = slotCtx.MouseScreenPos;
                    ShowOrRefreshSelectUI(pos, slotCtx);
                };
                
               
            }
        }
        
        private void ShowOrRefreshSelectUI(Vector2 screenPos, SlotContext ctx)
        {
            CloseSelectUI();
            currentSelectUIView = Instantiate(selectPrefab, parentCanvas.transform);
            currentSelectUIView.SetPosition(screenPos, parentCanvas);
            
        }
        
        private void CloseSelectUI()
        {
            if (currentSelectUIView != null)
            {
                Destroy(currentSelectUIView.gameObject);
                currentSelectUIView = null;
            }
        }
        
        
        private void RefreshItemUI()
        {
            var slot = VM.CurrentItemSlot;  
            var template = VM.CurrentItemData;
            
            sumBinder?.SetModel(template);
            countBinder?.SetModel(template);
            detailBinder?.SetModel(template);
            
            if (slot is ItemSlot itemSlot)
            {
                sumBinder?.SetModel(itemSlot);  
                countBinder?.SetModel(itemSlot); 
                detailBinder?.SetModel(itemSlot);
            }
        }

        private void ClearItemUI()
        {
            detailBinder?.SetModel((ItemSlot)null);
            sumBinder?.SetModel((ItemSlot)null);
            countBinder?.SetModel((ItemSlot)null);
        }
    }
}