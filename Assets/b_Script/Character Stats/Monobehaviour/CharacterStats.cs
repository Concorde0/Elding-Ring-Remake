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

    private void Awake()
    {
        
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
        int damage = Mathf.Max(attacker.CurrentDamage());
        CurrentHealth = Mathf.Max(CurrentHealth - damage);

        if (attacker.isCritical)
        {
            //TODO:触发受伤动画
        }
            
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
