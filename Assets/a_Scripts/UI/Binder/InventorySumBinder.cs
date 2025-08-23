using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class InventorySumBinder : MonoBehaviour
    {
        [Header("基础信息")]
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemName;
        
        
        [Header("使用说明")]
        [SerializeField] private TextMeshProUGUI descriptionText;

        public void SetModel(ItemData data)
        {
            itemIcon.sprite = data?.物品图标;
            itemIcon.enabled = data?.物品图标 != null;

            itemName.text = data?.名字 ?? string.Empty;
            
            descriptionText.text = data?.道具使用 ?? string.Empty;
            
            
        }
    }
}