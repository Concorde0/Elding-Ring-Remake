using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class InventoryCountBinder : MonoBehaviour
    {
        [Header("数量与消耗")]
        [SerializeField] private TextMeshProUGUI useCountText;
        [SerializeField] private TextMeshProUGUI totalCountText;
        [SerializeField] private TextMeshProUGUI heldCountText;
        [SerializeField] private TextMeshProUGUI focusCostText;

        public void SetModel(ItemData data)
        {
            useCountText.text    = data != null ? data.使用次数 : string.Empty;
            totalCountText.text  = data != null ? data.共有数.ToString() : string.Empty;
            heldCountText.text   = data != null ? data.持有数.ToString() : string.Empty;
            focusCostText.text   = data != null ? data.消耗专注值.ToString() : string.Empty;
        }
    }
}