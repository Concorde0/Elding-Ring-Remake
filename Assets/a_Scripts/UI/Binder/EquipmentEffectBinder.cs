using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentEffectBinder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI effectText1;
    [SerializeField] private TextMeshProUGUI effectText2;

    public void SetModel(EquipmentData data)
    {
        effectText1.text = data.附加效果1;
        effectText2.text = data.附加效果2;
    }
}
