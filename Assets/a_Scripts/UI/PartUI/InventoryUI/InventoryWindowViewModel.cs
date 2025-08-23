using System;
using System.Collections.Generic;
using System.Linq;
using RPG.UI;
using UnityEngine;

public class InventoryWindowViewModel : UIBaseViewModel
{
    private readonly InventoryModel _model;

    // 默认分类 ID 列表（示例）
    private readonly Dictionary<int, string[]> _defaultCategoryIds = new Dictionary<int, string[]>
    {
        { 0, new[] { "2001" } },
        { 1, new[] { "2001" } },
        { 2, new[] { "2001" } },
        { 3, new[] { "2001" } }
    };
    
    public event Action<int> OnCategoryChanged
    {
        add    => _model.OnCategoryChanged += value;
        remove => _model.OnCategoryChanged -= value;
    }
    public event Action OnItemChanged;

    public int CurrentCategory => _model.CurrentCategory;
    public ItemData CurrentItemData => _model.Item;

    public InventoryWindowViewModel() : this(new InventoryModel()) { }

    public InventoryWindowViewModel(InventoryModel model)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
    }

    public override void Initialize()
    {
        //按ID生成每个分类下的 IItemSlotData 列表
        foreach (var kv in _defaultCategoryIds)
        {
            var list = kv.Value.Select(GameDataProvider.GetItemById).Where(e => e != null).Cast<IItemSlotData>().ToList();
            _model.SetItemsForCategory(kv.Key, list);
        }
        
        _model.Initialize();
        SetEquipmentById("2001");
    }
    
    public void SwitchCategory(int newIndex)
    {
        _model.SwitchCategory(newIndex);
    }
    
    public void SetEquipmentById(string itemId)
    {
        var data = GameDataProvider.GetItemById(itemId);
        if (data == null)
        {
            Debug.LogWarning($"找不到Item ID={itemId}");
            return;
        }
        _model.SetItem(data);
        OnItemChanged?.Invoke();
    }

    public List<IItemSlotData> GetItemsByCategory(int categoryIndex)
    {
        return _model.GetItemsByCategory(categoryIndex);
    }
    
    
}