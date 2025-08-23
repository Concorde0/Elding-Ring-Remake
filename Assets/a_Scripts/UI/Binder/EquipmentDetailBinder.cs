using TMPro;
using UnityEngine;

public class EquipmentDetailBinder : MonoBehaviour
{
    [Header("攻击属性")]
    [SerializeField] private TextMeshProUGUI physicalAttackText;
    [SerializeField] private TextMeshProUGUI magicAttackText;
    [SerializeField] private TextMeshProUGUI fireAttackText;
    [SerializeField] private TextMeshProUGUI criticalAttackText;

    [Header("防御属性")]
    [SerializeField] private TextMeshProUGUI physicalDefenseText;
    [SerializeField] private TextMeshProUGUI magicDefenseText;
    [SerializeField] private TextMeshProUGUI fireDefenseText;
    [SerializeField] private TextMeshProUGUI poiseText;

    [Header("其他属性")]
    [SerializeField] private TextMeshProUGUI weightText;

    public void SetModel(EquipmentData data)
    {
        if (data == null)
        {
            ClearUI();
            return;
        }
        // 攻击属性
        physicalAttackText.text   = data != null ? $"物理 {data.a物理}" : "-";
        magicAttackText.text      = data != null ? $"魔力 {data.a魔力}" : "-";
        fireAttackText.text       = data != null ? $"火 {data.a火}" : "-";
        criticalAttackText.text   = data != null ? $"致命一击 {data.致命一击}" : "-";

        // 防御属性
        physicalDefenseText.text  = data != null ? $"物理 {data.d物理}" : "-";
        magicDefenseText.text     = data != null ? $"魔力 {data.d魔力}" : "-";
        fireDefenseText.text      = data != null ? $"火 {data.d火}" : "-";
        poiseText.text            = data != null ? $"防御强度 {data.防御强度}" : "-";

        // 其他属性
        weightText.text           = data != null ? $"重量 {data.重量}" : "-";
    }
    
    private void ClearUI()
    {
        physicalAttackText?.SetText("");
        magicAttackText?.SetText("");
        fireAttackText?.SetText("");
        criticalAttackText?.SetText("");
        physicalDefenseText?.SetText("");
        magicDefenseText?.SetText("");
        fireDefenseText?.SetText("");
        poiseText?.SetText("");
        weightText?.SetText("");
    }
    
}