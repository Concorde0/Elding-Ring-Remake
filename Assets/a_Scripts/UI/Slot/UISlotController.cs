using System;
using RPG.UI;
using UnityEngine;

namespace RPG.UI
{
    /// <summary>
    /// 负责把 SlotContext（包含 IItemSlotData）与 UISlotView 连接起来，
    /// 并把 SlotContext 原样抛出给上层（InventoryWindowView）。
    /// 现在支持 ItemSlot 实例（带 StackCount/InstanceId）。
    /// </summary>
    public class UISlotController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UISlotView view;

        private SlotContext context;

        public event Action<SlotContext> OnLeftClick;
        public event Action<SlotContext> OnRightClick;
        public event Action<SlotContext> OnHoverEnter;
        public event Action<SlotContext> OnHoverExit;

        private void Awake()
        {
            if (view == null)
                view = GetComponentInChildren<UISlotView>(true);
        }

        public void Initialize(SlotContext ctx, object unused = null)
        {
            BindView(ctx);
        }

        private void BindView(SlotContext ctx)
        {
            UnbindView();

            context = ctx;
            view.SetIcon(context.ItemData?.Icon);
            view.SetHighlight(false);

            view.OnLeftClick  += OnViewLeftClick;
            view.OnRightClick += OnViewRightClick;
            view.OnHoverEnter += OnViewHoverEnter;
            view.OnHoverExit  += OnViewHoverExit;
        }

        public void Refresh(SlotContext newContext)
        {
            // 更新 context 并刷新显示（例如图标）
            context = newContext;
            view.SetIcon(context.ItemData?.Icon);
        }

        private void UnbindView()
        {
            if (view == null) return;
            view.OnLeftClick  -= OnViewLeftClick;
            view.OnRightClick -= OnViewRightClick;
            view.OnHoverEnter -= OnViewHoverEnter;
            view.OnHoverExit  -= OnViewHoverExit;
        }

        private void OnViewLeftClick(UnityEngine.Vector2 mousePos)
        {
            OnLeftClick?.Invoke(CreateContext(mousePos));
        }

        private void OnViewRightClick(UnityEngine.Vector2 mousePos)
        {
            OnRightClick?.Invoke(CreateContext(mousePos));
        }

        private void OnViewHoverEnter()
        {
            view.SetHighlight(true);
            OnHoverEnter?.Invoke(CreateContext(UnityEngine.Input.mousePosition));
        }

        private void OnViewHoverExit()
        {
            view.SetHighlight(false);
            OnHoverExit?.Invoke(CreateContext(UnityEngine.Input.mousePosition));
        }

        private SlotContext CreateContext(UnityEngine.Vector2 mousePos)
        {
            return new SlotContext(
                context.SlotIndex,
                context.ItemData,
                mousePos,
                context.SlotRect
            );
        }

        private void OnDestroy()
        {
            UnbindView();
        }
    }
}
