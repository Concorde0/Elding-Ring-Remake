    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class EquipmentSumBinder : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI amplifyValue;
        [SerializeField] private TextMeshProUGUI itemType;

        public void SetModel(EquipmentData data)
        {
            itemIcon.sprite = data?.物品图标;
            itemIcon.enabled = data?.物品图标 != null;
            
            itemName.text = data?.名字 ?? string.Empty;
            
            amplifyValue.text = data != null ? $" {data.增幅}" : string.Empty;
            
            itemType.text = data != null ? $"{data.攻击类型}" : string.Empty;
        }
    }