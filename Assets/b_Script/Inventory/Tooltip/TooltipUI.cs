using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipUI : MonoBehaviour
{
    public static ToolTipUI Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject root;  // 整个面板的根节点（拖一个Panel上来）
    public Image icon;
    public Image icon2;
    public TextMeshProUGUI itemName;
    // public TextMeshProUGUI itemAmount;

    [Header("Details")]
    public TextMeshProUGUI t1;
    public TextMeshProUGUI t2;
    public TextMeshProUGUI t3;
    public TextMeshProUGUI t4;

    [Header("Attack")]
    public TextMeshProUGUI a1;
    public TextMeshProUGUI a2;
    public TextMeshProUGUI a3;
    public TextMeshProUGUI a4;

    [Header("Defense")]
    public TextMeshProUGUI d1;
    public TextMeshProUGUI d2;
    public TextMeshProUGUI d3;
    public TextMeshProUGUI d4;

    [Header("Ability Buffs")]
    public TextMeshProUGUI c1;
    public TextMeshProUGUI c2;
    public TextMeshProUGUI c3;
    public TextMeshProUGUI c4;
    public TextMeshProUGUI c5;

    [Header("Requirements")]
    public TextMeshProUGUI m1;
    public TextMeshProUGUI m2;
    public TextMeshProUGUI m3; 
    public TextMeshProUGUI m4;
    public TextMeshProUGUI m5;

    [Header("Other")]
    public TextMeshProUGUI description;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Show(ItemData_SO item)
    {
        if (item == null)
        {
            return;
        }

        root.SetActive(true);

        icon.sprite = item.itemIcon;
        icon2.sprite = item.itemIcon2;
        itemName.text = item.itemName;

        // Details
        t1.text = $"{item.t1}";
        t2.text = $"{item.t2}";
        t3.text = $"{item.t3}";
        t4.text = $"重量   {item.t4}";

        // Attack
        a1.text = $"物理   {item.a1}";
        a2.text = $"魔力   {item.a2}";
        a3.text = $"火   {item.a3}";
        a4.text = $"致命一击   {item.a4}";

        // Defense
        d1.text = $"物理   {item.d1}";
        d2.text = $"魔力   {item.d2}";
        d3.text = $"火   {item.d3}";
        d4.text = $"防御强度   {item.d4}";

        // Ability Buffs
        c1.text = $"力气   {item.c1}";
        c2.text = $"灵巧   {item.c2}";
        c3.text = $"智力   {item.c3}";
        c4.text = $"信仰   {item.c4}";
        c5.text = $"感应   {item.c5}";

        // Requirements
        m1.text = $"力气   {item.m1}";
        m2.text = $"灵巧   {item.m2}";
        m3.text = $"智力   {item.m3}";
        m4.text = $"信仰   {item.m4}";
        m5.text = $"感应   {item.m5}";

        description.text = item.description;
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}
