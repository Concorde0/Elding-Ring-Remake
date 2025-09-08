using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    
    [Header(("Weapon"))]
    public Transform weaponSlot;
    [HideInInspector]
    public bool isCritical;
    
    private float currentPoise = 0f;
    private float lastHitTime  = -999f;
    
    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Time.time - lastHitTime > characterData.poiseRecoveryTime)
        {
            currentPoise = Mathf.MoveTowards(currentPoise, 0f, Time.deltaTime * 10f);
        }
    }

    public void AddPoise(float amount)
    {
        currentPoise += amount;
        lastHitTime = Time.time;
    }
    
    public bool CheckStagger()
    {
        return currentPoise >= characterData.staggerThreshold && currentPoise < characterData.executionThreshold;
    }

    public bool CheckExecution()
    {
        return currentPoise >= characterData.executionThreshold;
    }

    public void ResetPoise()
    {
        currentPoise = 0f;
    }

    

    #region Read from Data_SO
    public int MaxHealth{
        get { if(characterData != null) return characterData.maxHealth;else return 0; }
        set { characterData.maxHealth = value; }
    }
    
    public int CurrentHealth{
        get { if(characterData != null) return characterData.currentHealth;else return 0; }
        set { characterData.currentHealth = value; }
    }
    
    public int MaxFocus{
        get { if(characterData != null) return characterData.maxFocus;else return 0; }
        set { characterData.maxFocus = value; }
    }
    
    public int CurrentFocus{
        get { if(characterData != null) return characterData.currentFocus;else return 0; }
        set { characterData.currentFocus = value; }
    }
    
    public int MaxEnergy{
        get { if(characterData != null) return characterData.maxEnergy;else return 0; }
        set { characterData.maxEnergy = value; }
    }
    
    public int CurrentEnergy{
        get { if(characterData != null) return characterData.currentEnergy;else return 0; }
        set { characterData.currentEnergy = value; }
    }
    
    #endregion
    
    #region Character Combat
    public void TakeDamage(CharacterStats attacker)
    {
        if (characterData == null || attackData == null) return;

        // 计算伤害
        int damage = Mathf.Max(attacker.CurrentDamage(), 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        // 加削韧值
        AddPoise(attacker.attackData.poiseDamage);

        // 判定硬直/处决
        if (CheckExecution())
        {
            // 进入待处决状态（行为树 Condition 会检测到）
            isCritical = true; 
        }
        else if (CheckStagger())
        {
            
        }
    
        // TODO: 如果血量 <= 0，则走死亡逻辑
    }
    

    private int CurrentDamage()
    {
        float Damage = attackData.Damage;
        
        return (int)Damage;
    }

    #endregion
    

    #region Apply Data Change

    public void ApplyHealth(int amount)
    {
        if (CurrentHealth + amount <= MaxHealth)
        {
            CurrentHealth += amount;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
    }
    
    public void ApplyFocus(int amount)
    {
        if (CurrentFocus + amount <= MaxFocus)
        {
            CurrentFocus += amount;
        }
        else
        {
            CurrentFocus = MaxFocus;
        }
    }
    

    #endregion
}
