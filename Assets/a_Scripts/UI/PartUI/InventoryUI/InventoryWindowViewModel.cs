using System;
using System.Collections.Generic;
using System.Linq;
using RPG.UI;
using UnityEngine;

public class InventoryWindowViewModel : UIBaseViewModel
{
    private readonly InventoryModel _model;
    private readonly string[] _defaultSeedIds = new[] { "2001", "2002" };

    public event Action<int> OnCategoryChanged
    {
        add    => _model.OnCategoryChanged += value;
        remove => _model.OnCategoryChanged -= value;
    }

    public event Action OnItemChanged;

    public int CurrentCategory => _model.CurrentCategory;
    
    public ItemData CurrentItemData
    {
        get
        {
            if (_model.Item == null) return null;
            if (_model.Item is ItemSlot slot) return slot.Template;
            return _model.Item as ItemData;
        }
    }
    public IItemSlotData CurrentItemSlot => _model.Item;

    public int SlotsPerCategory => _model.SlotsPerCategory;

    public InventoryWindowViewModel() : this(new InventoryModel()) { }

    public InventoryWindowViewModel(InventoryModel model)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
    }

    public override void Initialize()
    {
        var buckets = new List<List<IItemSlotData>>();
        for (int i = 0; i < 4; i++) buckets.Add(new List<IItemSlotData>());

        foreach (var id in _defaultSeedIds)
        {
            var tpl = GameDataProvider.GetItemById(id);

            int catIdx = Mathf.Clamp((int)tpl.catrgory, 0, 3);
            buckets[catIdx].Add(new ItemSlot(tpl));
        }

        for (int i = 0; i < buckets.Count; i++)
        {
            var list = buckets[i];
            if (list.Count > _model.SlotsPerCategory)
                list = list.Take(_model.SlotsPerCategory).ToList();

            while (list.Count < _model.SlotsPerCategory)
                list.Add(null);

            _model.SetItemsForCategory(i, list);
        }

        _model.Initialize();
        
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
        _model.SetItem(new ItemSlot(data));
        OnItemChanged?.Invoke();
    }
    
    public void SelectSlot(IItemSlotData slot)
    {
        _model.SetItem(slot);
        OnItemChanged?.Invoke();
    }
    
    public void ClearSelection()
    {
        _model.SetItem((ItemData)null);
        OnItemChanged?.Invoke();
    }

    public List<IItemSlotData> GetItemsByCategory(int categoryIndex)
    {
        return _model.GetItemsByCategory(categoryIndex);
    }

    public void AddItem(ItemData data)
    {
        OnItemChanged?.Invoke();
    }
    
    public void ReassignSlotsByTemplateCategory()
    {
        var moves = new List<(int fromCat, int fromIndex, ItemSlot slot)>();

        for (int cat = 0; cat < 4; cat++)
        {
            var list = _model.GetItemsByCategory(cat);
            for (int i = 0; i < list.Count; i++)
            {
                var it = list[i] as ItemSlot;
                if (it == null) continue;
                int targetCat = Mathf.Clamp((int)it.Template.catrgory, 0, 3);
                if (targetCat != cat)
                    moves.Add((cat, i, it));
            }
        }

        foreach (var m in moves)
        {
            bool ok = _model.AddItemToCategory((int)m.slot.Template.catrgory, m.slot);
            if (ok)
            {
                _model.ClearSlot(m.fromCat, m.fromIndex);
            }
        }
    }
    
}
