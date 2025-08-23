using System;
using System.Collections;
using System.Collections.Generic;
using RPG.UI;
using UnityEngine;

public class CharacterWindowViewModel : UIBaseViewModel
{
    private CharacterModel _model;
    public CharacterData CurrentCharacterData => _model.CharacterData;
    
    public event Action OnCharacterDataChanged;
    
    public CharacterWindowViewModel()
    {
        _model = new CharacterModel();
    }
    
    public override void Initialize()
    {
        OnCharacterDataChanged?.Invoke();
        SetCharacterDataById();
    }

    public void SetCharacterDataById()
    {
        var data = GameDataProvider.GetCharacterData;
        if (data == null)
        {
            Debug.LogWarning($"找不到角色data");
            return;
        }

        _model.SetCharacterData(data);
        OnCharacterDataChanged?.Invoke();
        
    }
}
