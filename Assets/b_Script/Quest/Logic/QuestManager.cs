using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [System.Serializable]
    public class QuestTask
    {
        public QuestData_SO questData;
        public bool IsStarted {get{return questData.isStarted;}set{questData.isStarted = value;}}
        public bool IsCompleted {get{return questData.isCompleted;}set{questData.isCompleted = value;}}
        public bool IsFinished {get{return questData.isFinished;}set{questData.isFinished = value;}}
    }
    public List<QuestTask> tasks = new List<QuestTask>();

    //敌人死亡，拾取物品
    public void UpdateQuestProgress(string requireName, int amount)
    {
        foreach (var task in tasks)
        {
            var matchTask = task.questData.questRequires.Find(r => r.name == requireName);
            if (matchTask != null)
            {
                matchTask.currentAmount += amount;
            }
            task.questData.CheckQuestProgress();
        }
    }

    public bool HaveQuest(QuestData_SO data)
    {
        if (data != null)
        {
            return tasks.Any(q => q.questData.questName == data.questName);
        }
        else
        {
            return false;
        }
    }

    public QuestTask GetTask(QuestData_SO data)
    {
        return tasks.Find(q => q.questData.questName == data.questName);
    }
}
