using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int currentLevel;
    public int lun;  
    
    public int vitality;
    public int focus;
    public int endurance;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int faith;
    public int induction;
    
    
    public int maxHealth;
    public int currentHealth;
    
    public int currentFocus;
    public int maxFocus;
    public int energy;
    
    public int currentWeight;
    public int maxWeight;
    
    public int maxLevel = 225;
    
    

 
}
