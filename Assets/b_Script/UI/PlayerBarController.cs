using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class PlayerBarController : MonoBehaviour
{
    [System.Serializable]
    public class BarGroup
    {
        public Image background;
        public Image bar;
        public Image frame;
        public float pixelsPerUnit = 4f; // 每个 max 单位对应多少像素宽度

        [HideInInspector] public RectTransform bgRect, barRect, frameRect;
        public void Init()
        {
            if (background) bgRect = background.rectTransform;
            if (bar) barRect = bar.rectTransform;
            if (frame) frameRect = frame.rectTransform;
        }
    }

    public BarGroup health;
    public BarGroup focus;
    public BarGroup energy;

    [Header("数据源")]
    public CharacterStats playerStats;
    public CharacterData_SO playerDataSO;

    void Start() { InitAll(); RefreshAll(); }
    void OnValidate() { InitAll(); RefreshAll(); }

    private void InitAll()
    {
        health?.Init();
        focus?.Init();
        energy?.Init();
    }

    private void Update()
    {
        RefreshAll();
    }

    public void RefreshAll()
    {
        if (playerStats == null || playerDataSO == null) return;
        UpdateGroup(health, playerStats.CurrentHealth, playerDataSO.maxHealth);
        UpdateGroup(focus,  playerStats.CurrentFocus,  playerDataSO.maxFocus);
        UpdateGroup(energy, playerStats.CurrentEnergy, playerDataSO.maxEnergy);
    }

    private void UpdateGroup(BarGroup g, float current, float max)
    {
        if (g == null) return;
        if (max <= 0f) max = 0.0001f;
        float t = Mathf.Clamp01(current / max);
        float totalWidth = max * g.pixelsPerUnit;
        
        if (g.bgRect != null)
        {
            var s = g.bgRect.sizeDelta; s.x = totalWidth; g.bgRect.sizeDelta = s;
        }
        if (g.frameRect != null)
        {
            var s = g.frameRect.sizeDelta; s.x = totalWidth; g.frameRect.sizeDelta = s;
        }
        
        if (g.bar != null && g.bar.type == Image.Type.Filled)
        {
            g.bar.fillAmount = t;
        }
        else if (g.barRect != null)
        {
            var s = g.barRect.sizeDelta; s.x = totalWidth * t; g.barRect.sizeDelta = s;
        }
    }
    
    public void RefreshNow() => RefreshAll();
}
