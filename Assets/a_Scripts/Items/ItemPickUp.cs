using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private string itemId;

    private ItemData _itemData;
    private bool _playerInRange;

    private void Start()
    {
        _itemData = GameDataProvider.GetItemById(itemId);
        if (_itemData == null)
            Debug.LogError($"找不到物品数据：{itemId}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            PickupUI.Instance.ShowPrompt(transform.position, _itemData.Id);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            PickupUI.Instance.HidePrompt();
        }
    }

    private void Update()
    {
        if (_playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        InventoryManager.Instance.AddItemToInventory(_itemData);
        PickupUI.Instance.HidePrompt();
        Destroy(gameObject);
    }
}
