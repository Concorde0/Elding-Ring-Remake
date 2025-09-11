using UnityEngine;
using TMPro;

public class CharacterPanelUI : MonoBehaviour
{
    public static CharacterPanelUI Instance { get; private set; }

    [Header("基本信息")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI lunText;

    [Header("血量 / 专注 / 重量")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI focusText;
    public TextMeshProUGUI weightText;

    [Header("属性")] 
    public TextMeshProUGUI vitalityText;
    public TextMeshProUGUI concentrationText;
    public TextMeshProUGUI enduranceText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI dexterityText;
    public TextMeshProUGUI intelligenceText;
    public TextMeshProUGUI faithText;
    public TextMeshProUGUI inductionText;
    public TextMeshProUGUI energyText;

    [Header("引用角色")]
    public CharacterStats characterStats;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        gameObject.SetActive(false); 
    }


    public void SetCharacter(CharacterStats stats)
    {
        if (stats == null) return;
        characterStats = stats;
        SafeRefreshUI();
    }
    
    public void SafeRefreshUI()
    {
        if (characterStats == null || characterStats.characterData == null) return;
        if (levelText == null) return;

        var data = characterStats.characterData;
        
        levelText.text = $"等级: {data.currentLevel}";
        lunText.text = $"持有卢恩: {data.lun}";
        
        healthText.text = $"血量: {data.currentHealth}/{data.maxHealth}";
        focusText.text = $"专注值: {data.currentFocus}/{data.maxFocus}";
        weightText.text = $"装备重量: {data.currentWeight}/{data.maxWeight}";
        
        vitalityText.text = $"生命力: {data.vitality}";
        concentrationText.text = $"专注力: {data.focus}";
        enduranceText.text = $"耐力: {data.endurance}";
        strengthText.text = $"力量: {data.strength}";
        dexterityText.text = $"灵巧: {data.dexterity}";
        intelligenceText.text = $"智力: {data.intelligence}";
        faithText.text = $"信仰: {data.faith}";
        inductionText.text = $"感应: {data.induction}";
        energyText.text = $"体力: {data.maxEnergy}";
    }
    
    public void ShowPanel()
    {
        SafeRefreshUI();
        gameObject.SetActive(true);
    }
    
    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
