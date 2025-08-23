using System;
using UnityEngine;

namespace RPG.UI
{
    public class UISlotController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HoverClickable clickable;
        [SerializeField] private UISlotView view;

        private SlotContext context;
        private PlayerStatsWindowViewModel targetVM;

        public event Action<SlotContext> OnLeftClick;
        public event Action<SlotContext> OnRightClick;
        public event Action<SlotContext> OnHoverEnter;
        public event Action<SlotContext> OnHoverExit;

        public void Initialize(SlotContext ctx, PlayerStatsWindowViewModel vm)
        {
            context = ctx;
            targetVM = vm;

            view.SetIcon(context.ItemData?.Icon);

            clickable.OnHoverEnter.RemoveAllListeners();
            clickable.OnHoverExit.RemoveAllListeners();
            clickable.OnLeftClick.RemoveAllListeners();
            clickable.OnRightClick.RemoveAllListeners();

            // 悬停：联动装备详情 + 向外转发
            clickable.OnHoverEnter.AddListener(() =>
            {
                if (context.ItemData is EquipmentData equipData)
                    targetVM?.SetEquipment(equipData);

                OnHoverEnter?.Invoke(new SlotContext(
                    context.SlotIndex,
                    context.ItemData,
                    Input.mousePosition,
                    context.SlotRect
                ));
            });

            clickable.OnHoverExit.AddListener(() =>
            {
                OnHoverExit?.Invoke(context);
            });

            // 左键
            clickable.OnLeftClick.AddListener(() =>
            {
                Debug.Log($"[UISlotController] Slot {ctx.SlotIndex} clicked");
                var updated = new SlotContext(
                    context.SlotIndex,
                    context.ItemData,
                    Input.mousePosition,
                    context.SlotRect
                );
                OnLeftClick?.Invoke(updated);
            });

            // 右键
            clickable.OnRightClick.AddListener(() =>
            {
                var updated = new SlotContext(
                    context.SlotIndex,
                    context.ItemData,
                    Input.mousePosition,
                    context.SlotRect
                );
                OnRightClick?.Invoke(updated);
            });
        }

        public void Refresh()
        {
            view.SetIcon(context.ItemData?.Icon);
        }
    }
}
