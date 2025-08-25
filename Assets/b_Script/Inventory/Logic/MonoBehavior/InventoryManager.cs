using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    //嵌套类，用来存储原始背包数据以及背包格位置。这里的数据在DragItems中一一赋值了
    //另外，这里的嵌套类应迁移到DragItem中，嵌套类最好在类内使用，不对外开放，这里的嵌套类的实际使用是在DragItem中
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }
    
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
    
    
    [Header("Drag Canvases")]
    public Canvas dragCanvas;
    public DragData currentDrag;
    
    
    [Header("UI Panels")]
    public GameObject bagPanel;
    public GameObject statsPanel;
    
    private bool isOpen = false;
    
    [Header("Stats Texts")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    
    [Header("Tooltips")]
    public ItemToolTip tooltip;

    private void Start()
    {
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }
    private void Update()
    {
        //开关背包
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statsPanel.SetActive(isOpen);
        }
        //传入三个数据用来展示人物面板的人物数据
        UpdateStatsText(
            GameManager.Instance.playerStats.MaxHealth,
            GameManager.Instance.playerStats.attackData.minDamage,
            GameManager.Instance.playerStats.attackData.maxDamage);
    }

    private void UpdateStatsText(int health , int min,int max)
    {
        healthText.text = health.ToString();
        attackText.text = min + "/" + max;
    }

    
    #region 检查拖拽物品是否在每一个Slot范围内

    public bool CheckInInventoryUI(Vector3 position)
    {
        for (int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            var t = inventoryUI.slotHolders[i].transform as RectTransform;
            
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
    
    public bool CheckInActionUI(Vector3 position)
    {
        foreach (var t1 in inventoryUI.slotHolders)
        {
            var t = t1.transform as RectTransform;
            
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }

        return false;
    }
    
    public bool CheckInEquipmentUI(Vector3 position)
    {
        foreach (var t1 in inventoryUI.slotHolders)
        {
            var t = t1.transform as RectTransform;
            
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }

        return false;
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
