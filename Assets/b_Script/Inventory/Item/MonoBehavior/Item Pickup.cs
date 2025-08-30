using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityInput;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    
    public ItemData_SO itemData;
    public GameObject keyDownUI;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enter trigger");
            keyDownUI.SetActive(true);
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
                QuestManager.Instance.UpdateQuestProgress(itemData.itemName,itemData.itemAmount);
                keyDownUI.SetActive(false);
                Destroy(gameObject);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            keyDownUI.SetActive(false);
        }
    }

    
}
