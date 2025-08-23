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
        physicalAttackText.text   = data.a物理.ToString();
        magicAttackText.text      = data.a魔力.ToString();
        fireAttackText.text       = data.a火.ToString();
        criticalAttackText.text   = data.致命一击.ToString();

        // 防御属性
        physicalDefenseText.text  = data.d物理.ToString();
        magicDefenseText.text     = data.d魔力.ToString();
        fireDefenseText.text      = data.d火.ToString();
        poiseText.text            = data.防御强度.ToString();

        // 其他属性
        weightText.text           = data.重量.ToString();
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