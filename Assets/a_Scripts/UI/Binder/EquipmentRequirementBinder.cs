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
        // 能力加成
        strength.text     = data.力气.ToString();
        agility.text      = data.灵巧.ToString();
        intelligence.text = data.智力.ToString();
        faith.text        = data.信仰.ToString();
        perception.text   = data.感应.ToString();

        // 必须能力值
        Mstrength.text     = data.m力气.ToString();
        Magility.text      = data.m灵巧.ToString();
        Mintelligence.text = data.m智力.ToString();
        Mfaith.text        = data.m信仰.ToString();
        Mperception.text   = data.m感应.ToString();
    }
}