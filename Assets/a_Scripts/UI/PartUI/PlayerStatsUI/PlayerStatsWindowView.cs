using System;
using UnityEngine;

namespace RPG.UI
{
    public class PlayerStatsWindowView : UIBaseView
    {
        public PlayerStatsWindowViewModel VM => ViewModel as PlayerStatsWindowViewModel;

        [Header("Equipment UI Binders")]
        [SerializeField] private EquipmentDetailBinder detailBinder;
        [SerializeField] private EquipmentEffectBinder effectBinder;
        [SerializeField] private EquipmentIconBinder iconBinder;
        [SerializeField] private EquipmentRequirementBinder requirementBinder;
        [SerializeField] private EquipmentSkillBinder skillBinder;
        [SerializeField] private EquipmentSumBinder sumBinder;

        [Header("Other References")]
        [SerializeField] private Canvas parentCanvas;       
        [SerializeField] private SelectUIView selectPrefab;
        
        private SelectUIView currentSelectUIView;
        
        private bool _eventsBound = false;
        
        protected override void BindEvents()
        {
            if (VM == null)
            {
                _eventsBound = false;
                return;
            }

            if (_eventsBound)
            {
                return;
            }

            // 先移除再添加，防止重复订阅
            VM.OnEquipmentChanged -= RefreshEquipmentUI;
            VM.OnEquipmentChanged += RefreshEquipmentUI;
            RegisterSelectEvents();

            _eventsBound = true;
            RefreshEquipmentUI();
        }

        protected override void UnbindEvents()
        {
            if (! _eventsBound)
                return;

            if (VM != null)
            {
                VM.OnEquipmentChanged -= RefreshEquipmentUI;
            }
            _eventsBound = false;
        }
        
        private void RefreshEquipmentUI()
        {
            if (VM == null)
            {
                return;
            }
            var data = VM.CurrentEquipmentData;
            
            if (data == null) 
            {
                detailBinder?.SetModel(null);
                effectBinder?.SetModel(null);
                iconBinder?.SetModel(null);
                requirementBinder?.SetModel(null);
                skillBinder?.SetModel(null);
                sumBinder?.SetModel(null);
                return;
            }
            
            detailBinder?.SetModel(data);
            effectBinder?.SetModel(data);
            iconBinder?.SetModel(data);
            requirementBinder?.SetModel(data);
            skillBinder?.SetModel(data);
            sumBinder?.SetModel(data);
        }

        private void RegisterSelectEvents()
        {
            var clickables = GetComponentsInChildren<HoverClickable>(true);
            foreach (var c in clickables)
            {
                c.OnLeftClick.RemoveAllListeners();
                c.OnLeftClick.AddListener(ShowOrRefreshSelectUI);
            }
        }
        
        private void ShowOrRefreshSelectUI()
        {
            Vector2 pos = Input.mousePosition;

            if (currentSelectUIView != null)
            {
                Destroy(currentSelectUIView.gameObject);
            }

            currentSelectUIView = Instantiate(selectPrefab, parentCanvas.transform);
            currentSelectUIView.SetPosition(pos, parentCanvas);
        }
        
    }
}
