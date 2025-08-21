using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Game Data/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("基础信息")] 
    public string Id; 
    public string DisplayName; 
    public Sprite Portrait; // 头像

    [Header("初始属性")] 
    public int BaseHP;
    public int BaseMP;
    public int BaseSP;
    public int BaseAttack;
    public int BaseDefense;

    [TextArea] public string Description;
} 
