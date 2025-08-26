using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    Weapon,Armor,Accessories,Others,Head,Chest,Legs,Hands
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;
    public GameObject itemPrefab;
    [TextArea]
    public string description = "";
    public bool stackable;
    public bool useAble;
    
    [FormerlySerializedAs("itemData")] [Header("Useable Items")]
    public UseableItemData_SO useableData;
    
    [Header("Weapon")]
    public GameObject weaponPrefab;

    public AttackData_SO weaponData;

    public AnimatorOverrideController weaponAnimator;
}
