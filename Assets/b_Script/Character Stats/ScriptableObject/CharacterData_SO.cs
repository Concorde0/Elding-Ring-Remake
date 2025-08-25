using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefense;
    public int currentDefense;
    
    [Header("Kill")]
    public int killPoint;
    
    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;

    private float LevelMultiplier
    {
        get{ return 1 + (currentLevel -1) * levelBuff;}
    }

    public void UpdateExp(int point)
    {
        currentExp += point;

        if (currentExp >= baseExp)
        {
            LeveUp();
        }
    }

    private void LeveUp()
    {
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        baseExp += (int)(baseExp * LevelMultiplier);
        maxHealth = (int)(maxHealth * LevelMultiplier);
        currentHealth = maxHealth;
        Debug.Log("LEVEL UP!" + currentLevel + "Max Health:" + maxHealth);
    }
}
