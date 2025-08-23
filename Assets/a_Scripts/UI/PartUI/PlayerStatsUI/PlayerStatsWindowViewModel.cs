using System;
using UnityEngine;

namespace RPG.UI
{
    public class PlayerStatsWindowViewModel : UIBaseViewModel
    {
        private readonly PlayerStatsModel _model;

        public event Action OnEquipmentChanged;
        public EquipmentData CurrentEquipmentData => _model.Equipment;

        public PlayerStatsWindowViewModel()
        {
            _model = new PlayerStatsModel();
        }

        public override void Initialize()
        {
            OnEquipmentChanged?.Invoke();
            SetEquipmentById("1001");
            
        }

        public void SetEquipmentById(string equipId)
        {
            
            var data = GameDataProvider.GetEquipmentById(equipId);
            if (data == null)
            {
                Debug.LogWarning($"找不到装备 ID={equipId}");
                return;
            }
            _model.SetEquipment(data);
            OnEquipmentChanged?.Invoke();
        }

        public void SetEquipment(EquipmentData data)
        {
            if (data == null) return;
            _model.SetEquipment(data);
            OnEquipmentChanged?.Invoke();
        }
    }
}