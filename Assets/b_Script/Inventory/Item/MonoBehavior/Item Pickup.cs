using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    
    public ItemData_SO itemData;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enter trigger");
            InventoryManager.Instance.inventoryData.AddItem(itemData,itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            // GameManager.Instance.playerStats.EquipWeapon(itemData);        
            QuestManager.Instance.UpdateQuestProgress(itemData.itemName,itemData.itemAmount);
            Destroy(gameObject);
        }
    }
}
