using System;
using System.Collections;
using System.Collections.Generic;
using RPG.MotionSystem;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    
    [Header(("Weapon"))]
    public Transform weaponSlot;

    [HideInInspector]
    public bool hasPlayedStagger = false;
    public bool isCritical;
    public bool isExecution;
    
    private float currentPoise = 0f;
    private float lastHitTime  = -999f;
    public bool shouldPlayExecutionAnim = false;
    
    public WeaponHitbox weaponHitbox;
    
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
    
    public void ResetStaggerFlag()
    {
        hasPlayedStagger = false;
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
    public void TakeDamage(CharacterStats attacker, bool forceSpecial = false) {
        if (characterData == null || attackData == null) return;

        int damage = Mathf.Max(attacker.CurrentDamage(), 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        AddPoise(attacker.attackData.poiseDamage);
        
        if (CheckExecution()) {
            isExecution = true;
        } else if (CheckStagger()) {
            if (!hasPlayedStagger) {
                isCritical = true;
                hasPlayedStagger = true;
            }
        }
        
        if (CurrentHealth <= 0) {
            var tree = GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
        }
    }

    public void OpenSpecialHurt()
    {
        weaponHitbox.triggerSpecialHurt = true;
    }
    
    public void CloseSpecialHurt()
    {
        weaponHitbox.triggerSpecialHurt = false;
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
