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
        var item = SafeGetItem(slot);

        useButton.interactable = (item != null && (item.isConsumable || IsEquipItem(item)));
        dropButton.interactable = (item != null);

        menuRoot.SetActive(true);

        RectTransform parentRect = menuRect.parent as RectTransform;
        if (parentRect == null) { menuRect.position = screenPosition; }
        else
        {
            menuRect.pivot = new Vector2(0f, 1f);
            Canvas rootCanvas = menuRoot.GetComponentInParent<Canvas>();
            Camera cam = (rootCanvas != null && rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay) ? rootCanvas.worldCamera : null;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPosition, cam, out var localPoint);
            menuRect.anchoredPosition = localPoint;
        }

        menuRoot.transform.SetAsLastSibling();

        if (blocker != null)
        {
            blocker.gameObject.SetActive(false);
            StartCoroutine(EnableBlockerNextFrame());
        }
    }

    private IEnumerator EnableBlockerNextFrame()
    {
        yield return null;
        if (blocker == null) yield break;
        blocker.gameObject.SetActive(true);
        var menuSibling = menuRoot.transform.GetSiblingIndex();
        blocker.transform.SetSiblingIndex(Mathf.Max(0, menuSibling));
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
    if (currentSlot == null) { Close(); return; }
    ItemData_SO item = SafeGetItem(currentSlot);
    if (item == null) { Close(); return; }

    var bag = currentSlot.itemUI.Bag;
    int idx = currentSlot.itemUI.Index;
    if (bag == null || idx < 0 || idx >= bag.items.Count) { Close(); return; }

    //装备逻辑
    if (IsEquipItem(item))
    {
        var equipmentBag = InventoryManager.Instance.equipmentData;
        if (equipmentBag == null) { Close(); return; }
        
        int[] candidateIndices;
        switch (item.itemType)
        {
            case ItemType.Weapon:      candidateIndices = new int[]{ 0, 1, 2, 5, 6, 7 }; break;
            case ItemType.Head:        candidateIndices = new int[]{ 10 }; break;
            case ItemType.Chest:       candidateIndices = new int[]{ 11 }; break;
            case ItemType.Hands:       candidateIndices = new int[]{ 12 }; break;
            case ItemType.Legs:        candidateIndices = new int[]{ 13 }; break;
            case ItemType.Accessories: candidateIndices = new int[]{ 15, 16, 17 }; break;
            default:                   candidateIndices = new int[0]; break;
        }

        // 找第一个空槽
        int targetIndex = -1;
        foreach (var i in candidateIndices)
        {
            if (i >= 0 && i < equipmentBag.items.Count && equipmentBag.items[i].itemData == null)
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex < 0) { Debug.Log("没有空的装备槽"); Close(); return; }

        // 放入装备槽
        equipmentBag.items[targetIndex].itemData = item;
        equipmentBag.items[targetIndex].amount = 1;
        
        bag.items[idx].amount--;
        if (bag.items[idx].amount <= 0)
        {
            bag.items[idx].amount = 0;
            bag.items[idx].itemData = null;
        }
    }
    //消耗品逻辑
    else if (item.isConsumable)
    {
        if (GameManager.Instance != null && GameManager.Instance.playerStats != null)
        {
            if (item.useableData != null)
                GameManager.Instance.playerStats.ApplyHealth(item.useableData.healthPoint);
        }

        bag.items[idx].amount--;
        if (bag.items[idx].amount <= 0)
        {
            bag.items[idx].amount = 0;
            bag.items[idx].itemData = null;
        }
    }

    RefreshAllContainers();
    Close();
}
    

    private void OnDropClicked()
    {
        if (currentSlot == null) { Close(); return; }
        ItemData_SO item = SafeGetItem(currentSlot);
        if (item == null) { Close(); return; }

        var bag = currentSlot.itemUI.Bag;
        int idx = currentSlot.itemUI.Index;
        if (bag != null && idx >= 0 && idx < bag.items.Count)
        {
            bag.items[idx].amount = Mathf.Max(0, bag.items[idx].amount - 1);
            if (bag.items[idx].amount == 0) bag.items[idx].itemData = null;
        }

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
            InventoryManager.Instance.inventoryUI?.RefreshUI();
            InventoryManager.Instance.actionUI?.RefreshUI();
            InventoryManager.Instance.equipmentUI?.RefreshUI();
        }
    }

    private bool IsEquipItem(ItemData_SO item)
    {
        return item != null && !item.isConsumable && 
               (item.itemType == ItemType.Weapon ||
                item.itemType == ItemType.Head ||
                item.itemType == ItemType.Chest ||
                item.itemType == ItemType.Hands ||
                item.itemType == ItemType.Legs ||
                item.itemType == ItemType.Accessories);
    }
}
