using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 运行时的“格子实例”数据。包装 ItemData（模板）并附带实例字段。
/// 实现 IItemSlotData 以便 UI 仍按模板显示（Id/DisplayName/Icon）——兼容旧代码。
/// </summary>
[Serializable]
public class ItemSlot : IItemSlotData
{
    // 模板引用（只读）
    public ItemData Template;

    // 每个格子独立的数据（可扩展）
    public string InstanceId;    // 格子唯一实例 ID（GUID）
    public int StackCount;       // 堆叠数量（对不可堆叠物品可为1）
    public float Durability;     // 耐久（0~100 仅作示例）
    public bool IsEquipped;      // 是否被装备（若用于装备槽）
    public List<SerializableKV> CustomData; // 可扩展键值对（Unity 可序列化）

    // 构造
    public ItemSlot(ItemData template, int stack = 1)
    {
        Template = template;
        StackCount = Math.Max(1, stack);
        InstanceId = Guid.NewGuid().ToString();
        Durability = 100f;
        IsEquipped = false;
        CustomData = new List<SerializableKV>();
    }

    // 供反序列化使用
    public ItemSlot() { CustomData = new List<SerializableKV>(); }

    // IItemSlotData 实现（返回模板相关显示信息）
    string IItemSlotData.Id => Template != null ? Template.Id : string.Empty;
    string IItemSlotData.DisplayName => Template != null ? Template.名字 : string.Empty;
    UnityEngine.Sprite IItemSlotData.Icon => Template != null ? Template.物品图标 : null;

    // DTO 转换（方便存档）
    public ItemSlotDTO ToDTO()
    {
        return new ItemSlotDTO
        {
            TemplateId = Template != null ? Template.Id : null,
            InstanceId = InstanceId,
            StackCount = StackCount,
            Durability = Durability,
            IsEquipped = IsEquipped,
            CustomData = CustomData != null ? CustomData.ToArray() : new SerializableKV[0]
        };
    }

    public static ItemSlot FromDTO(ItemSlotDTO dto)
    {
        if (dto == null) return null;
        var template = string.IsNullOrEmpty(dto.TemplateId) ? null : GameDataProvider.GetItemById(dto.TemplateId);
        var slot = new ItemSlot
        {
            Template = template,
            InstanceId = string.IsNullOrEmpty(dto.InstanceId) ? Guid.NewGuid().ToString() : dto.InstanceId,
            StackCount = Math.Max(1, dto.StackCount),
            Durability = dto.Durability,
            IsEquipped = dto.IsEquipped,
            CustomData = dto.CustomData != null ? new List<SerializableKV>(dto.CustomData) : new List<SerializableKV>()
        };
        return slot;
    }
}

/// <summary>
/// 简单可序列化键值对（Dictionary 不可直接被 Unity 序列化）
/// </summary>
[Serializable]
public struct SerializableKV
{
    public string Key;
    public string Value;
    public SerializableKV(string k, string v) { Key = k; Value = v; }
}

/// <summary>
/// DTO 用于持久化存档（Json-friendly）
/// </summary>
[Serializable]
public class ItemSlotDTO
{
    public string TemplateId;
    public string InstanceId;
    public int StackCount;
    public float Durability;
    public bool IsEquipped;
    public SerializableKV[] CustomData;
}
