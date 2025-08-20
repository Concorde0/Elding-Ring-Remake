using UnityEngine;

namespace RPG.UI
{
    public class PlayerStatsWindowView : UIBaseView
    {
        private PlayerStatsWindowViewModel VM => ViewModel as PlayerStatsWindowViewModel;

        protected override void BindEvents()
        {
            VM.OnStatsUpdated += RefreshStatsUI;
        }

        protected override void UnbindEvents()
        {
            if (VM != null)
                VM.OnStatsUpdated -= RefreshStatsUI;
        }

        protected override void OnInitialized()
        {
            // 如果有初始化 UI 逻辑，可以写在这里
        }

        protected override void OnShow()
        {
            base.OnShow();
            RefreshStatsUI();
        }

        /// <summary>
        /// 刷新 UI 显示玩家属性
        /// </summary>
        private void RefreshStatsUI()
        {
            // TODO: 根据 VM 的数据更新 UI
            // 例如：strengthText.text = VM.Strength.ToString();
            //       agilityText.text = VM.Agility.ToString();
        }
    }
}