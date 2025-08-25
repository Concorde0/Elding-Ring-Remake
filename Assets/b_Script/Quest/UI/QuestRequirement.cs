using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private TextMeshProUGUI requireName;
    private TextMeshProUGUI progressNumber;

    private void Awake()
    {
        requireName = GetComponent<TextMeshProUGUI>();
        //获取到第一个子物体身上的TMP
        progressNumber = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetupRequirement(string name, int amount, int currentAmount)
    {
        requireName.text = name;
        progressNumber.text = currentAmount.ToString() + "/" + amount.ToString();
    }

    public void SetupRequirement(string name, bool isFinished)
    {
        if (isFinished == true)
        {
            requireName.text = name;
            progressNumber.text = "complete";
            requireName.color = Color.gray;
            progressNumber.color = Color.gray;
        }
    }
}
