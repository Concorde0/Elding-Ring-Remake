using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentIconBinder : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetModel(EquipmentData data)
    {
        icon.sprite = data?.物品图标;
    }
}

