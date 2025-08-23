using UnityEngine;

namespace RPG.UI
{
    /// <summary>
    /// 装备信息面板 View，负责将 ViewModel 的数据分发给各个 Binder
    /// </summary>
    public class PlayerStatsWindowView : UIBaseView
    {
        private PlayerStatsWindowViewModel VM => ViewModel as PlayerStatsWindowViewModel;

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

        
        protected override void BindEvents()
        {
            
            if (VM != null)
            {
                VM.OnEquipmentChanged += RefreshEquipmentUI;
            }
            
            SelectUIEvent();
        }

        protected override void UnbindEvents()
        {
            if (VM != null)
                VM.OnEquipmentChanged -= RefreshEquipmentUI;
        }

        /// <summary>
        /// 刷新所有装备信息 Binder
        /// </summary>
        private void RefreshEquipmentUI()
        {
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

        /// <summary>
        /// 为所有 HoverClickable 绑定打开 SelectUI 的事件
        /// </summary>
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
