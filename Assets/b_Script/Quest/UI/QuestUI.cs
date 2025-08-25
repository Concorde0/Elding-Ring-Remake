using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Elements")]
    public GameObject questPanel;
    public ItemToolTip tooltip;
    private bool isOpen;
    
    [Header("Quest Name")]
    public RectTransform questListTransform;
    public QuestNameButton questNameButton;
    
    [Header("Text Content")]
    public TextMeshProUGUI questContentText;

    [Header("Requirement")] 
    public RectTransform requireTransform;
    public QuestRequirement requirement;
    
    [Header("Reward Panel")]
    public RectTransform rewardTransform;

    public ItemUI rewardUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);
            questContentText.text = "";

            SetupQuestList();
            if (!isOpen)
            {
                tooltip.gameObject.SetActive(false);
            }
        }
    }

    private void SetupQuestList()
    {
        foreach (Transform item in questListTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in rewardTransform)
        {
            Destroy(item.gameObject);
        }
        
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var task in QuestManager.Instance.tasks)
        {
            var newTask = Instantiate(questNameButton, questListTransform);
            newTask.SetupNameButton(task.questData);
            newTask.questContentText = questContentText;
        }
        
    }

    public void SetupRequireList(QuestData_SO questData)
    {
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (var require in questData.questRequires)
        {
            var q = Instantiate(requirement, requireTransform);
            if (questData.isFinished)
            {
                q.SetupRequirement(require.name,true);
            }
            else
            {
                q.SetupRequirement(require.name, require.requiredAmount,require.currentAmount);
            }
        }
    }

    public void SetupRewardItem(ItemData_SO itemData, int amount)
    {
        var item = Instantiate(rewardUI, rewardTransform);
        item.SetupItemUI(itemData, amount);
    }
}
