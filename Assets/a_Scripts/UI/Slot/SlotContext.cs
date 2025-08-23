using UnityEngine;

public readonly struct SlotContext
{
    public readonly int SlotIndex;
    public readonly IItemSlotData ItemData;       // 数据层引用（SO/实体）
    public readonly Vector2 MouseScreenPos;       // 鼠标位置
    public readonly RectTransform SlotRect;       // UI Rect

    public SlotContext(
        int slotIndex,
        IItemSlotData itemData,
        Vector2 mouseScreenPos,
        RectTransform slotRect)
    {
        SlotIndex = slotIndex;
        ItemData = itemData;
        MouseScreenPos = mouseScreenPos;
        SlotRect = slotRect;
    }
}