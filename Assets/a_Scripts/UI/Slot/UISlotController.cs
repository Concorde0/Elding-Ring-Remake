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
            
            clickable.OnHoverEnter.RemoveAllListeners();
            clickable.OnHoverExit .RemoveAllListeners();
            clickable.OnLeftClick  .RemoveAllListeners();
            clickable.OnRightClick .RemoveAllListeners();
            
            clickable.OnHoverEnter.AddListener(() =>
            {
                OnHoverEnter?.Invoke(CreateContext());
            });
            
            clickable.OnHoverExit.AddListener(() =>
            {
                OnHoverExit?.Invoke(CreateContext());
            });
            
            clickable.OnLeftClick.AddListener(() =>
            {
                OnLeftClick?.Invoke(CreateContext());
            });
            
            clickable.OnRightClick.AddListener(() =>
            {
                OnRightClick?.Invoke(CreateContext());
            });
        }
        
        // 外部如果需要刷新同一个槽位的 Icon 或 ItemData，可以调用此方法
        public void Refresh(SlotContext newContext)
        {
            context = newContext;
            view.SetIcon(context.ItemData?.Icon);
        }

       
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
