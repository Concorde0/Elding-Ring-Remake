using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory
}

[CreateAssetMenu(fileName = "NewEquipmentData", menuName = "Game Data/Equipment Data")]
public class EquipmentData : ScriptableObject
{
    [Header("基础信息")]
    public string Id;
    public string DisplayName;
    public EquipmentType Type;
    public Sprite Icon;

    [Header("属性加成")]
    public int AttackBonus;
    public int DefenseBonus;
    public int HPBonus;
    public int MPBonus;

    [TextArea]
    public string Description;
}