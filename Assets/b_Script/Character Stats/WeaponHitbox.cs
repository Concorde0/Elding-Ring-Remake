using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    [Header("Owner Stats")]
    public CharacterStats ownerStats;
    
    public bool triggerSpecialHurt = false;
    
    //记录本次攻击已经命中过的目标
    private HashSet<CharacterStats> hitTargets = new HashSet<CharacterStats>();

    private void Awake()
    {
    }

    private void OnEnable()
    {
        hitTargets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ownerStats == null) return;

        CharacterStats targetStats = other.GetComponent<CharacterStats>();
        if (targetStats != null && targetStats != ownerStats)
        {
            if (!hitTargets.Contains(targetStats))
            {
                
                hitTargets.Add(targetStats);

                //是否特殊受伤
                targetStats.TakeDamage(ownerStats, triggerSpecialHurt);
            }
        }
    }
}