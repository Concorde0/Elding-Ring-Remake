using UnityEngine;

public interface IItemSlotData
{
    string Id { get; }                  // 唯一标识符，用于查找详情
    string DisplayName { get; }         // UI展示名称
    Sprite Icon { get; }                // 图标
    

}