using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Game Data/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("基础信息")] 
    public int 等级; 
    public int 持有卢恩; 
    
    [Header("初始属性")] 
    public int 生命力; 
    public int 集中力;
    public int 耐力;
    public int 力气;
    public int 灵巧;
    public int 智力;
    public int 信仰;
    public int 感应;
    
    [Header("状态")]
    public int currentHP;
    public int maxHP;
    public int currentFP;
    public int maxFP;
    public int current精力;
    public int max精力;
    public int current负重;
    public int max负重;
    
    
} 
