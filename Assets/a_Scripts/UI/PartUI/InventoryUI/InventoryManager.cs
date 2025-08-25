using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public InventoryWindowViewModel InventoryVM { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InventoryVM = new InventoryWindowViewModel();
            InventoryVM.Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddItemToInventory(ItemData data)
    {
        InventoryVM.AddItem(data);
    }
}
