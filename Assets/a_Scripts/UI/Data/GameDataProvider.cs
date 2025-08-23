using UnityEngine;

public static class GameDataProvider
{
    
    private static EquipmentDatabase _db;
    private static EquipmentDatabase DB
    {
        get
        {
            
            if (_db == null)
            {
                _db = Resources.Load<EquipmentDatabase>("Data/EquipmentDatabase");
                if (_db == null)
                    Debug.LogError("加载 EquipmentDatabase 失败！");
            }
            return _db;
        }
    }
    
    private static ItemDataBase _itemDb;
    private static ItemDataBase ItemDB
    {
        get
        {
            if (_itemDb == null)
            {
                _itemDb = Resources.Load<ItemDataBase>("Data/ItemDataBase");
                if (_itemDb == null)
                    Debug.LogError("加载 ItemDatabase 失败！");
            }
            return _itemDb;
        }
    }

    private static CharacterData _characterData;
    public static CharacterData GetCharacterData
    {
        get
        {
            if (_characterData == null)
            {
                _characterData = Resources.Load<CharacterData>("Data/CharacterData");
                if (_characterData == null)
                    Debug.LogError("加载 CharacterData 失败！");
            }
            return _characterData;
        }
    }

    public static EquipmentData GetEquipmentById(string id)
    {
        return DB == null ? null : DB.GetById(id);
    }
    
    public static ItemData GetItemById(string id)
    {
        return ItemDB == null ? null : ItemDB.GetById(id);
    }
}