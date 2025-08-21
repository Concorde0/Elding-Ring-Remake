using System;
using UnityEngine;

namespace RPG.UI
{
    public class UISlotController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HoverClickable clickable; // 新增引用
        [SerializeField] private UISlotView view;

        private SlotContext context;

        public event Action<SlotContext> OnHoverEnter;
        public event Action<SlotContext> OnHoverExit;
        public event Action<SlotContext> OnLeftClick;
        public event Action<SlotContext> OnRightClick;

        public void Initialize(SlotContext ctx)
        {
            context = ctx;

            // 先解绑避免重复订阅
            clickable.OnHoverEnter.RemoveAllListeners();
            clickable.OnHoverExit.RemoveAllListeners();
            clickable.OnLeftClick.RemoveAllListeners();
            clickable.OnRightClick.RemoveAllListeners();

            // 事件转发，并带上下文
            clickable.OnHoverEnter.AddListener(() => OnHoverEnter?.Invoke(context));
            clickable.OnHoverExit.AddListener(() => OnHoverExit?.Invoke(context));
            clickable.OnLeftClick.AddListener(() => OnLeftClick?.Invoke(context));
            clickable.OnRightClick.AddListener(() => OnRightClick?.Invoke(context));
            
            
        }
    }
}