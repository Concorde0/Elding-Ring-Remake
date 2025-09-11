using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentData;

  
    [Header("Containers")] 
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;
    
    
    [Header("UI Panels")]
    public GameObject bagPanel;
    public GameObject statsPanel;
    
    private bool isBagOpen = false;
    private bool isStatsOpen = false;
    
    [Header("物品")]
    public InventoryRouter router;

    [Header("共享UI")]
    public ContainerUI bagUI;

    protected override void Awake()
    {
        base.Awake();
        router.Init();
        bagUI.BindData(router.accessoriesData);
    }

    private void Start()
    {
        if (equipmentUI != null && equipmentData != null)
            equipmentUI.BindData(equipmentData);
        inventoryUI.RefreshUI();
        equipmentUI.RefreshUI();
        CharacterPanelUI.Instance.SetCharacter(GameManager.Instance.playerStats);
        // actionUI.RefreshUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBagOpen = !isBagOpen;
            isStatsOpen = false;

            bagPanel.SetActive(isBagOpen);
            statsPanel.SetActive(isStatsOpen);
            
            CharacterPanelUI.Instance.gameObject.SetActive(isBagOpen || isStatsOpen);
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            isStatsOpen = !isStatsOpen;
            isBagOpen = false;

            statsPanel.SetActive(isStatsOpen);
            bagPanel.SetActive(isBagOpen);

            CharacterPanelUI.Instance.gameObject.SetActive(isBagOpen || isStatsOpen);
        }
    }
    
    #region UI切页
    public void OnWeaponTabClicked() => bagUI.BindData(router.weaponData);
    public void OnArmorTabClicked() => bagUI.BindData(router.armorData);
    public void OnAccessoriesTabClicked() => bagUI.BindData(router.accessoriesData);
    public void OnOthersTabClicked() => bagUI.BindData(router.othersData);
    #endregion

    #region 物品操作
    public void AddItem(ItemData_SO item, int amount = 1)
    {
        if (item == null || router == null) return;

        var targetData = router.GetDataForItem(item);
        if (targetData != null)
        {
            targetData.AddItem(item, amount);
            if (bagUI.inventoryData == targetData)
            {
                bagUI.RefreshUI();
            }
                
        }
    }
    
    
    
    #endregion

    #region 检测任务物品

    public void CheckQuestItemInBag(string questItemName)
    {
        foreach (var item in inventoryData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.name == questItemName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName, item.itemData.itemAmount);
                }
            }
        }
        
        foreach (var item in actionData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.name == questItemName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName, item.itemData.itemAmount);
                }
            }
        }
    }
    

    #endregion
    
    //检测背包和快捷栏物品
    public InventoryItem QuestItemInBag(ItemData_SO questItem)
    {
        return inventoryData.items.Find(i => i.itemData == questItem);
    }
    
    public InventoryItem QuestItemInAction(ItemData_SO questItem)
    {
        return actionData.items.Find(i => i.itemData == questItem);
    }
}
