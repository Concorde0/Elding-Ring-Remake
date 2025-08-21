using UnityEngine;
using RPG.UI;

public readonly struct SlotContext
{
    public readonly int SlotIndex;              // 第几个格子
    public readonly IItemSlotData ItemData;     // 当前物品数据
    public readonly Vector2 MouseScreenPos;     // 点击时鼠标位置
    public readonly RectTransform SlotRect;     // 槽位的 RectTransform

    public SlotContext(int slotIndex, IItemSlotData itemData, Vector2 mouseScreenPos, RectTransform slotRect)
    {
        SlotIndex = slotIndex;
        ItemData = itemData;
        MouseScreenPos = mouseScreenPos;
        SlotRect = slotRect;
    }
}