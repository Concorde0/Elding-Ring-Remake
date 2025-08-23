using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentData", menuName = "Game Data/Equipment Data")]
public class EquipmentData : ScriptableObject
{
    [Header("基础信息")]
    public string Id;
    public string 名字;
    public string 增幅;
    public string 攻击类型;
    public string 战技;
    public int 消耗专注值;
    public int 重量;
    public Sprite 物品图标;
    public Sprite 战技图标;

    [Header("攻击力")] 
    public int a物理;
    public int a魔力;
    public int a火;
    public int 致命一击;
    
    [Header("防御时减伤率")]
    public int d物理;
    public int d魔力;
    public int d火;
    public int 防御强度;
    
    [Header("能力加成")]
    public int 力气;
    public int 灵巧;
    public int 智力;
    public int 信仰;
    public int 感应;
    
    [Header("必须能力值")]
    public int m力气;
    public int m灵巧;
    public int m智力;
    public int m信仰;
    public int m感应;
    
    [Header("附加效果")]
    public string 附加效果1;
    public string 附加效果2;
    public string 附加效果3;
    
    [TextArea]
    public string Description;
}