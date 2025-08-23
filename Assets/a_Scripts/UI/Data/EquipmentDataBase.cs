using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "Game Data/Equipment Database")]
public class EquipmentDatabase : ScriptableObject
{
    public List<EquipmentData> AllEquipments;

    public EquipmentData GetById(string id)
    {
        return AllEquipments.FirstOrDefault(e => e.Id == id);
    }
}