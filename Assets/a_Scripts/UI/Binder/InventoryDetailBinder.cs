using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class InventoryDetailBinder : MonoBehaviour
    {
        [Header("能力加成")]
        [SerializeField] private TextMeshProUGUI strengthText;
        [SerializeField] private TextMeshProUGUI dexterityText;
        [SerializeField] private TextMeshProUGUI intelligenceText;
        [SerializeField] private TextMeshProUGUI faithText;
        [SerializeField] private TextMeshProUGUI arcaneText;
        
        [Header("其他属性")]
        [SerializeField] private TextMeshProUGUI weightText;
        public void SetModel(ItemData data)
        {
            strengthText.text     = data != null ? data.力气     : string.Empty;
            dexterityText.text    = data != null ? data.灵巧     : string.Empty;
            intelligenceText.text = data != null ? data.智力     : string.Empty;
            faithText.text        = data != null ? data.信仰     : string.Empty;
            arcaneText.text       = data != null ? data.感应     : string.Empty;
            
            weightText.text       = data != null ? $"重量 {data.重量}" : "-";
        }
        
        public void SetModel(ItemSlot slot)
        {
            if (slot == null)
            {
                SetModel((ItemData)null);
                return;
            }

            SetModel(slot.Template);

            
        }
    }
}