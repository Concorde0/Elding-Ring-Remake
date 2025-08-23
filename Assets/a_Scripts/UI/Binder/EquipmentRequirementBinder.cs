using TMPro;
using UnityEngine;

public class EquipmentRequirementBinder : MonoBehaviour
{
    [Header("能力加成")]
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI agility;
    [SerializeField] private TextMeshProUGUI intelligence;
    [SerializeField] private TextMeshProUGUI faith;
    [SerializeField] private TextMeshProUGUI perception;

    [Header("必须能力值")]
    [SerializeField] private TextMeshProUGUI Mstrength;
    [SerializeField] private TextMeshProUGUI Magility;
    [SerializeField] private TextMeshProUGUI Mintelligence;
    [SerializeField] private TextMeshProUGUI Mfaith;
    [SerializeField] private TextMeshProUGUI Mperception;

    public void SetModel(EquipmentData data)
    {
        if (data == null)
        {
            // 清空显示，避免 null 崩溃
            strength.text = agility.text = intelligence.text = faith.text = perception.text = "-";
            Mstrength.text = Magility.text = Mintelligence.text = Mfaith.text = Mperception.text = "-";
            return;
        }

        // 能力加成
        strength.text     = data != null ? $"力气 {data.力气}" : "-";
        agility.text      = data != null ? $"灵巧 {data.灵巧}" : "-";
        intelligence.text = data != null ? $"智力 {data.智力}" : "-";
        faith.text        = data != null ? $"信仰 {data.信仰}" : "-";
        perception.text   = data != null ? $"感应 {data.感应}" : "-";

        // 必须能力值
        Mstrength.text     = data != null ? $"力气 {data.m力气}" : "-";
        Magility.text      = data != null ? $"灵巧 {data.m灵巧}" : "-";
        Mintelligence.text = data != null ? $"智力 {data.m智力}" : "-";
        Mfaith.text        = data != null ? $"信仰 {data.m信仰}" : "-";
        Mperception.text   = data != null ? $"感应 {data.m感应}" : "-";
    }
}