using UnityEngine;

namespace RPG.UI
{
    public class PlayerStatsWindowView : UIBaseView
    {
        private PlayerStatsWindowViewModel VM => ViewModel as PlayerStatsWindowViewModel;

        [Header("References")]
        [SerializeField] private Canvas parentCanvas;       // UI所在Canvas
        [SerializeField] private SelectUIView selectPrefab; // SelectUI预制体

        protected override void BindEvents()
        {
            VM.OnStatsUpdated += RefreshStatsUI;

            SelectUIEvent();
            
            
        }
        protected override void UnbindEvents()
        {
            if (VM != null)
                VM.OnStatsUpdated -= RefreshStatsUI;
        }

        private void RefreshStatsUI()
        {
            // TODO: 刷新属性UI
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