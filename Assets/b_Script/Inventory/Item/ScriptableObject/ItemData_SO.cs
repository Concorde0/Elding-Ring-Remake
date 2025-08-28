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
    public Sprite itemIcon2;
    public int itemAmount;
    public GameObject itemPrefab;
    public bool stackable;
    public bool useAble;
    public bool isConsumable;
    
    public EquipmentSlot allowedSlot;
    
    
    [Header("Details")]
    public string t1;
    public string t2;
    public string t3;
    public string t4;
    [Header("Attack")] 
    public int a1;
    public int a2;
    public int a3;
    public int a4;
    [Header("Defense")]
    public int d1;
    public int d2;
    public int d3;
    public int d4;
    [Header("能力加成")]
    public string c1;
    public string c2;
    public string c3;
    public string c4;
    public string c5;
    [Header("必须能力值")] 
    public int m1;
    public int m2;
    public int m3;
    public int m4;
    public int m5;
    [TextArea]
    public string description = "";
    
    [FormerlySerializedAs("itemData")] 
    [Header("Useable Items")]
    public UseableItemData_SO useableData;
    
    [Header("Weapon")]
    public GameObject weaponPrefab;

    public AttackData_SO weaponData;

    public AnimatorOverrideController weaponAnimator;
}
