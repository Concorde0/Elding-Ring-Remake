using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterBinder : MonoBehaviour
{
    [Header("基础信息")] 
    [SerializeField]private TextMeshProUGUI 等级; 
    [SerializeField]private TextMeshProUGUI 持有卢恩; 
                    
    [Header("初始属性")]
    [SerializeField]private TextMeshProUGUI 生命力; 
    [SerializeField]private TextMeshProUGUI 集中力;
    [SerializeField]private TextMeshProUGUI 耐力;
    [SerializeField]private TextMeshProUGUI 力气;
    [SerializeField]private TextMeshProUGUI 灵巧;
    [SerializeField]private TextMeshProUGUI 智力;
    [SerializeField]private TextMeshProUGUI 信仰;
    [SerializeField]private TextMeshProUGUI 感应;
                    
    [Header("状态")] 
    [SerializeField]private TextMeshProUGUI currentHP;
    [SerializeField]private TextMeshProUGUI maxHP;
    [SerializeField]private TextMeshProUGUI currentFP;
    [SerializeField]private TextMeshProUGUI maxFP;
    [SerializeField]private TextMeshProUGUI current精力;
    [SerializeField]private TextMeshProUGUI max精力;
    [SerializeField]private TextMeshProUGUI current负重;
    [SerializeField]private TextMeshProUGUI max负重;
    
    public void SetModel(CharacterData data)
    {
        if (data == null)
        {
            ClearUI();
            return;
        }
        
        等级.text       = $"等级 {data.等级}";
        持有卢恩.text   = $"卢恩 {data.持有卢恩}";
        
        生命力.text     = $"生命力 {data.生命力}";
        集中力.text     = $"集中力 {data.集中力}";
        耐力.text       = $"耐力 {data.耐力}";
        力气.text       = $"力气 {data.力气}";
        灵巧.text       = $"灵巧 {data.灵巧}";
        智力.text       = $"智力 {data.智力}";
        信仰.text       = $"信仰 {data.信仰}";
        感应.text       = $"感应 {data.感应}";
        
        currentHP.text     = $"{data.currentHP}/{data.maxHP}";
        currentFP.text     = $"{data.currentFP}/{data.maxFP}";
        current精力.text   = $"{data.current精力}/{data.max精力}";
        current负重.text   = $"{data.current负重}/{data.max负重}";
    }
    
    private void ClearUI()
    {
        等级?.SetText("");
        持有卢恩?.SetText("");

        生命力?.SetText("");
        集中力?.SetText("");
        耐力?.SetText("");
        力气?.SetText("");
        灵巧?.SetText("");
        智力?.SetText("");
        信仰?.SetText("");
        感应?.SetText("");

        currentHP?.SetText("");
        maxHP?.SetText("");
        currentFP?.SetText("");
        maxFP?.SetText("");
        current精力?.SetText("");
        max精力?.SetText("");
        current负重?.SetText("");
        max负重?.SetText("");
    }
}
