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
        if (skillIcon != null)
            skillIcon.sprite = data?.战技图标;

        if (skillName != null)
            skillName.text = data?.战技 ?? "无战技";

        if (fpCost != null)
            fpCost.text = data != null ? $"消耗专注值 {data.消耗专注值}" : "消耗专注值: -";
    }
}

