using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire
    {
        public string name;
        public int requiredAmount;
        public int currentAmount;
    }
    public GameObject rewardPrefab;
    public int rewardAmount = 1;
    
    public string questName;
    [TextArea] 
    public string description;

    public bool isStarted;
    public bool isCompleted;
    public bool isFinished;
    
    [FormerlySerializedAs("questRequirements")] public List<QuestRequire> questRequires = new List<QuestRequire>();
    public List<InventoryItem> rewards = new List<InventoryItem>();

    public void CheckQuestProgress()
    {
        var finishRequires = questRequires.Where(r => r.requiredAmount <= r.currentAmount);
        isCompleted = finishRequires.Count() == questRequires.Count;

        if (isCompleted)
        {
            Debug.Log("任务完成");
            GiveRewards();
        }
    }

    // public void GiveRewards()
    // {
    //     
    //     foreach (var reward in rewards)
    //     {
    //         if (reward.amount < 0)
    //         {
    //             int requireCount = Mathf.Abs(reward.amount);
    //
    //             if (InventoryManager.Instance.QuestItemInBag(reward.itemData) != null)
    //             {
    //                 if (InventoryManager.Instance.QuestItemInBag(reward.itemData).amount <= requireCount)
    //                 {
    //                     requireCount -= InventoryManager.Instance.QuestItemInBag(reward.itemData).amount;
    //                     InventoryManager.Instance.QuestItemInBag(reward.itemData).amount = 0;
    //                     if (InventoryManager.Instance.QuestItemInAction(reward.itemData) != null)
    //                     {
    //                         InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
    //                     }
    //                 }
    //                 else
    //                 {
    //                     InventoryManager.Instance.QuestItemInBag(reward.itemData).amount -= requireCount;
    //                 }
    //             }
    //             else
    //             {
    //                 InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
    //             }
    //         }
    //         else
    //         {
    //             InventoryManager.Instance.AddItem(reward.itemData,reward.amount);
    //         }
    //         
    //         InventoryManager.Instance.inventoryUI.RefreshUI();
    //         InventoryManager.Instance.actionUI.RefreshUI();
    //     }
    // }
    
    public void GiveRewards()
    {
        if (rewardPrefab != null)
        {
            for (int i = 0; i < rewardAmount; i++)
            {
                Vector3 spawnPos = GetGroundPosition();
                Instantiate(rewardPrefab, spawnPos, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("未设置prefab");
        }

        InventoryManager.Instance.inventoryUI.RefreshUI();
        InventoryManager.Instance.actionUI.RefreshUI();
    }
    
    private Vector3 GetGroundPosition()
    {
        Vector3 origin = Camera.main.transform.position + Camera.main.transform.forward * 2f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 10f))
        {
            return hit.point + Vector3.up * 1f;
        }
        
        return origin + Vector3.up * 1f;
    }
    
    
    
    //当前任务需要 收集/消灭 的目标名字列表
    public List<string> RequireTargetNames()
    {
        List<string> targetNameList = new List<string>();
        foreach (var require in questRequires)
        {
            targetNameList.Add(require.name);
        }
        return targetNameList;
    }
}
