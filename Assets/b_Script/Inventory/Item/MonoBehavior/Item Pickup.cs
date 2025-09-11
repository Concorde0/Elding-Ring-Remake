using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityInput;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    
    public ItemData_SO itemData;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enter trigger");
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                InventoryManager.Instance.AddItem(itemData,itemData.itemAmount);
                InventoryManager.Instance.inventoryUI.RefreshUI();
                // QuestManager.Instance.UpdateQuestProgress(itemData.itemName,itemData.itemAmount);
                Destroy(gameObject);
            }
            
        }
    }
    

    
}
