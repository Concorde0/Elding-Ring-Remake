using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public enum Catrgory
{
    Accessories = 0,
    Item = 1,
    Weapon = 2,
    Armor = 3,
}


[CreateAssetMenu(fileName = "NewItemData", menuName = "Game Data/Item Data")]
public class ItemData : ScriptableObject,IItemSlotData
{
    [Header("基础信息")]
    public string Id;
    public string 名字;
    public Sprite 物品图标;
    public string 使用次数;
    public int 共有数;
    public int 持有数;
    public int 消耗专注值;
    public string 重量;
    public Catrgory catrgory;

    [TextArea]
    public string 道具使用;

    [Header("能力加成")]
    public string 力气;
    public string 灵巧;
    public string 智力;
    public string 信仰;
    public string 感应;

    // ----- 实现 IItemSlotData 接口 -----
    string IItemSlotData.Id   => Id;
    string IItemSlotData.DisplayName => 名字;
    Sprite IItemSlotData.Icon => 物品图标;
}
