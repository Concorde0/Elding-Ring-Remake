using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContextMenu : MonoBehaviour
{
    public static InventoryContextMenu Instance { get; private set; }

    [Header("UI")]
    public GameObject menuRoot;
    public Button useButton;
    public Button dropButton;
    public Button closeButton;
    public Button blocker; // 全屏透明Button用于点击关闭

    private RectTransform menuRect;
    private SlotHolder currentSlot;

    
    protected void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        menuRect = menuRoot.GetComponent<RectTransform>();
        HideImmediate();
        
        useButton.onClick.AddListener(OnUseClicked);
        dropButton.onClick.AddListener(OnDropClicked);
        closeButton.onClick.AddListener(Close);
        blocker.onClick.AddListener(Close);
    }

    protected void OnDestroy()
    {
        if (Instance == this) Instance = null;
        
    }
    
    public void Open(SlotHolder slot, Vector2 screenPosition)
    {
        if (slot == null) return;
        currentSlot = slot;
        // 更新Use按钮是否可用
        var item = SafeGetItem(slot);
        useButton.interactable = (item != null && item.useAble);
        dropButton.interactable = (item != null);

        menuRoot.SetActive(true);
        
        RectTransform parentRect = menuRect.parent as RectTransform;
        if (parentRect == null) { menuRect.position = screenPosition; return; }

        //将pivot设为左上
        menuRect.pivot = new Vector2(0f, 1f);

        //把屏幕坐标转换到父本地坐标
        Canvas rootCanvas = menuRoot.GetComponentInParent<Canvas>();
        Camera cam = (rootCanvas != null && rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay) ? rootCanvas.worldCamera : null;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPosition, cam, out localPoint);
        
        menuRect.anchoredPosition = localPoint;
    }

    private ItemData_SO SafeGetItem(SlotHolder slot)
    {
        if (slot == null || slot.itemUI == null) return null;
        try { return slot.itemUI.GetItem(); }
        catch { return null; }
    }

    public void Close()
    {
        currentSlot = null;
        menuRoot.SetActive(false);
        blocker.gameObject.SetActive(false);
    }

    private void HideImmediate()
    {
        if (menuRoot != null) menuRoot.SetActive(false);
        if (blocker != null) blocker.gameObject.SetActive(false);
    }
    
    private void OnUseClicked()
    {
        if (currentSlot == null)
        {
            Close(); return;
        }
        ItemData_SO item = SafeGetItem(currentSlot);
        if (item == null)
        {
            Close(); return;
        }

        var bag = currentSlot.itemUI.Bag;
        int idx = currentSlot.itemUI.Index;
        if (bag == null || idx < 0 || idx >= bag.items.Count)
        {
            Close(); return;
        }

        //装备逻辑
        if (item.itemType == ItemType.Weapon ||
            item.itemType == ItemType.Head ||
            item.itemType == ItemType.Chest ||
            item.itemType == ItemType.Hands ||
            item.itemType == ItemType.Legs ||
            item.itemType == ItemType.Accessories)
        {
            var equipmentBag = InventoryManager.Instance.equipmentData;
            if (equipmentBag == null)
            {
                Debug.Log("NULLL");
            }
            if (equipmentBag != null)
            {
                int equipIndex = GetEquipIndex(item.itemType);

                // 如果槽已有装备，丢回背包
                if (equipmentBag.items[equipIndex].itemData != null)
                {
                    bag.AddItem(equipmentBag.items[equipIndex].itemData, 1);
                }
                
                equipmentBag.items[equipIndex].itemData = item;
                equipmentBag.items[equipIndex].amount = 1;

                // 移除背包中这一件
                bag.items[idx].amount--;
                if (bag.items[idx].amount <= 0)
                    bag.items[idx].itemData = null;
            }
        }
        //消耗品逻辑
        else if (item.useAble)
        {
            if (GameManager.Instance != null && GameManager.Instance.playerStats != null)
            {
                if (item.useableData != null)
                {
                    GameManager.Instance.playerStats.ApplyHealth(item.useableData.healthPoint);
                }
            }

            bag.items[idx].amount--;
            if (bag.items[idx].amount <= 0)
                bag.items[idx].itemData = null;
        }

        RefreshAllContainers();
        Close();
    }
    
    private int GetEquipIndex(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon: return 0;
            case ItemType.Head: return 1;
            case ItemType.Chest: return 2;
            case ItemType.Hands: return 3;
            case ItemType.Legs: return 4;
            case ItemType.Accessories: return 5;
            default: return -1;
        }
    }

    private void OnDropClicked()
    {
        if (currentSlot == null) { Close(); return; }
        ItemData_SO item = SafeGetItem(currentSlot);
        if (item == null) { Close(); return; }

        //减少背包数据
        var bag = currentSlot.itemUI.Bag;
        int idx = currentSlot.itemUI.Index;
        if (bag != null && idx >= 0 && idx < bag.items.Count)
        {
            bag.items[idx].amount = Mathf.Max(0, bag.items[idx].amount - 1);
            if (bag.items[idx].amount == 0) bag.items[idx].itemData = null;
        }

        //在世界中生成 prefab
        if (item.itemPrefab != null && GameManager.Instance != null && GameManager.Instance.playerStats != null)
        {
            Transform playerT = GameManager.Instance.playerStats.transform;
            Vector3 spawnPos = playerT.position + playerT.forward * 1.2f + Vector3.up * 0.5f;
            GameObject.Instantiate(item.itemPrefab, spawnPos, Quaternion.identity);
        }

        RefreshAllContainers();
        Close();
    }

    private void RefreshAllContainers()
    {
        if (InventoryManager.Instance != null)
        {
            if (InventoryManager.Instance.inventoryUI != null) InventoryManager.Instance.inventoryUI.RefreshUI();
            if (InventoryManager.Instance.actionUI != null) InventoryManager.Instance.actionUI.RefreshUI();
            if (InventoryManager.Instance.equipmentUI != null) InventoryManager.Instance.equipmentUI.RefreshUI();
        }
    }
}
