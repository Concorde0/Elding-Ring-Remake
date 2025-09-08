using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Attack",menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public int Damage;
    public int poiseDamage;

    public void ApplyWeaponData(AttackData_SO weapon)
    {
        Damage = weapon.Damage;
    }
}
