using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    //手动拖入相对应的DATA，直接去调用SO中的唯一数据
    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentData;

    //手动拖入每个相关的UI组件，用ContainerUI中的refresh进行刷新物品格信息，或是可以通过ContainerUI去访问相对应的Slot信息，对应到每个格
    [Header("Containers")] 
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;
    
    
    [Header("UI Panels")]
    public GameObject bagPanel;
    public GameObject statsPanel;
    
    private bool isOpen = false;
    
    [Header("Stats Texts")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    
    [Header("Tooltips")]
    public ItemToolTip tooltip;
    
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
        inventoryUI.RefreshUI();
        // actionUI.RefreshUI();
        // equipmentUI.RefreshUI();
    }
    private void Update()
    {
        // 开关背包
         if (Input.GetKeyDown(KeyCode.B))
         {
             isOpen = !isOpen;
             bagPanel.SetActive(isOpen);
             statsPanel.SetActive(isOpen);
         }
         //传入三个数据用来展示人物面板的人物数据
         // UpdateStatsText(
         //     GameManager.Instance.playerStats.MaxHealth,
         //     GameManager.Instance.playerStats.attackData.Damage);
    }

    private void UpdateStatsText(int health , int damage)
    {
        healthText.text = health.ToString();
        attackText.text = damage.ToString();
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
            Debug.Log("ADD");
            targetData.AddItem(item, amount);
            if (bagUI.inventoryData == targetData)
            {
                bagUI.RefreshUI();
            }
                
        }
    }

    public void RemoveItem(ItemData_SO item, int amount = 1)
    {
        if (item == null || router == null) return;

        var targetData = router.GetDataForItem(item);
        if (targetData != null)
        {
            targetData.RemoveItem(item, amount);
            if (bagUI.inventoryData == targetData)
                bagUI.RefreshUI();
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
