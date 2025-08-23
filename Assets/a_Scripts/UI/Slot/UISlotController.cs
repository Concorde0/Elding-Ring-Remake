using System;
using UnityEngine;

namespace RPG.UI
{
    /// <summary>
    /// 最笨实现：不区分 EquipmentData / ItemData，
    /// 只负责显示图标、捕获鼠标事件并原样抛出 SlotContext
    /// </summary>
    public class UISlotController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HoverClickable clickable;
        [SerializeField] private UISlotView    view;

        private SlotContext context;

        public event Action<SlotContext> OnLeftClick;
        public event Action<SlotContext> OnRightClick;
        public event Action<SlotContext> OnHoverEnter;
        public event Action<SlotContext> OnHoverExit;

        /// <summary>
        /// 初始化时只传 SlotContext 即可，
        /// context.ItemData 可以是 EquipmentData 或 ItemData
        /// </summary>
        public void Initialize(SlotContext ctx, object unknown)
        {
            context = ctx;
            view.SetIcon(context.ItemData?.Icon);

            // 清掉所有旧的监听器
            clickable.OnHoverEnter.RemoveAllListeners();
            clickable.OnHoverExit .RemoveAllListeners();
            clickable.OnLeftClick  .RemoveAllListeners();
            clickable.OnRightClick .RemoveAllListeners();

            // Hover Enter
            clickable.OnHoverEnter.AddListener(() =>
            {
                OnHoverEnter?.Invoke(CreateContext());
            });

            // Hover Exit
            clickable.OnHoverExit.AddListener(() =>
            {
                OnHoverExit?.Invoke(CreateContext());
            });

            // 左键
            clickable.OnLeftClick.AddListener(() =>
            {
                OnLeftClick?.Invoke(CreateContext());
            });

            // 右键
            clickable.OnRightClick.AddListener(() =>
            {
                OnRightClick?.Invoke(CreateContext());
            });
        }

        /// <summary>
        /// 外部如果需要刷新同一个槽位的 Icon 或 ItemData，可以调用此方法
        /// </summary>
        public void Refresh(SlotContext newContext)
        {
            context = newContext;
            view.SetIcon(context.ItemData?.Icon);
        }

        /// <summary>
        /// 把最新的鼠标位置也封装进新的 SlotContext 返回
        /// </summary>
        private SlotContext CreateContext()
        {
            return new SlotContext(
                context.SlotIndex,
                context.ItemData,
                Input.mousePosition,
                context.SlotRect
            );
        }
    }
}
