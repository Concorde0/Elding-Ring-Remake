using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    
    public ItemData_SO EquippedWeapon;
    public ItemData_SO EquippedHead;
    public ItemData_SO EquippedChest;
    public ItemData_SO EquippedHands;
    public ItemData_SO EquippedLegs;
    public ItemData_SO EquippedAccessories;
    
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
    
    public int BaseDefense{
        get { if(characterData != null) return characterData.baseDefense;else return 0; }
        set { characterData.baseDefense = value; }
    }
    
    public int CurrentDefense{
        get { if(characterData != null) return characterData.currentDefense;else return 0; }
        set { characterData.currentDefense = value; }
    }
    #endregion
    
    #region Character Combat
    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefense,0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage,0);

        if (attacker.isCritical)
        {
            //TODO:触发受伤动画
        }

        if (CurrentHealth <= 0)
        {
            attacker.characterData.UpdateExp(characterData.killPoint);
        }
            
    }

    private int CurrentDamage()
    {
        float Damage = attackData.Damage;
        
        return (int)Damage;
    }

    #endregion
    
    #region Equip Weapon

    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.weaponPrefab != null)
        {
            Instantiate(weapon.weaponPrefab, weaponSlot);
        }
        attackData.ApplyWeaponData(weapon.weaponData);
        // GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
        
    }

    public void UnEquipWeapon()
    {
        if (weaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        }
        // attackData.ApplyWeaponData(baseAttackData);
        // GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
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
    

    #endregion
}
