using System;

namespace RPG.UI
{
    public class PlayerStatsWindowViewModel : UIBaseViewModel
    {
        
        public PlayerStatsWindowViewModel() 
        {
            _model = new PlayerStatsModel();
        }
        
        public event Action OnEquipmentChanged;
        private readonly PlayerStatsModel _model;
        public EquipmentData CurrentEquipmentData => _model.Equipment;
        

        public PlayerStatsWindowViewModel(PlayerStatsModel model)
        {
            _model = model;
        }

        public override void Initialize()
        {
            if (_model.Equipment == null)
            {
                _model.SetEquipment(new EquipmentData());
            }
            OnEquipmentChanged?.Invoke();
        }

        public void SetEquipment(EquipmentData data)
        {
            _model.SetEquipment(data);
            OnEquipmentChanged?.Invoke();
        }
    }
}