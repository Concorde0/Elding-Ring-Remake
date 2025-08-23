using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSkillBinder : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI fpCost;

    public void SetModel(EquipmentData data)
    {
        skillIcon.sprite = data?.战技图标;
        skillName.text = data?.战技 ?? "无战技";
        fpCost.text = $"消耗专注值: {data.消耗专注值}";
    }
}

