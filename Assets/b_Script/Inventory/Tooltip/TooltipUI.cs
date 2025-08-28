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
        // itemAmount.text = "x" + item.itemAmount;
        
        t1.text = item.t1;
        t2.text = item.t2;
        t3.text = item.t3;
        t4.text = item.t4;
    

        a1.text = item.a1.ToString();
        a2.text = item.a2.ToString();
        a3.text = item.a3.ToString();
        a4.text = item.a4.ToString();
        
        d1.text = item.d1.ToString();
        d2.text = item.d2.ToString();
        d3.text = item.d3.ToString();
        d4.text = item.d4.ToString();
        
        c1.text = item.c1;
        c2.text = item.c2;
        c3.text = item.c3;
        c4.text = item.c4;
        c5.text = item.c5;
        
        m1.text = item.m1.ToString();
        m2.text = item.m2.ToString();
        m3.text = item.m3.ToString();
        m4.text = item.m4.ToString();
        m5.text = item.m5.ToString();
        
        description.text = item.description;
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}
