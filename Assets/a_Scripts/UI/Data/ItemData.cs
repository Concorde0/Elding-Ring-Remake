using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewItemData", menuName = "Game Data/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("基础信息")]
    public string Id;
    public string 名字;
    public Sprite 物品图标;
    public string 使用次数;
    public int 共有数;
    public int 持有数;
    public int 消耗专注值;
    public int 重量;
    
    [TextArea]
    public string 道具使用;
    
    [Header("能力加成")]
    public string 力气;
    public string 灵巧;
    public string 智力;
    public string 信仰;
    public string 感应;
}
