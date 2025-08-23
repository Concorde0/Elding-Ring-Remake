using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Game Data/Item DataBase")]
public class ItemDataBase : ScriptableObject
{
    public List<ItemData> AllItems;

    public ItemData GetById(string id)
    {
        return AllItems.FirstOrDefault(e => e.Id == id);
    }
}
