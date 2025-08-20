using System;

namespace RPG.UI
{
    public class PlayerStatsWindowViewModel : UIBaseViewModel
    {
        public event Action OnStatsUpdated;
        

        public override void Initialize()
        {
            LoadPlayerStats();
        }

        private void LoadPlayerStats()
        {
            OnStatsUpdated?.Invoke();
        }

        // 外部调用：当属性变化时刷新 UI
        public void UpdateStats(int str, int agi, int intel)
        {
            OnStatsUpdated?.Invoke();
        }
    }
}