using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel
{
    public CharacterData CharacterData { get; private set; }

    public void SetCharacterData(CharacterData characterData)
    {
        CharacterData = CharacterData;
    }
}
