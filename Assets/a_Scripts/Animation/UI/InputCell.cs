using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;


namespace RPG.InputSystem
{
    public enum KeyType
    {
        Key,ValueKey,AxisPosKey,AxisNegKey,
    }
    public class InputCell : MonoBehaviour,IPointerClickHandler
    {
        public KeyType type;
        public string keyName;
        private TextMeshPro keyCode_Text;
        private Image keyBG_Image;
        private Action<KeyCode> SetKey;

        private void Awake()
        {
            InputCellManager.AddCell(this);
            keyBG_Image = GetComponent<Image>();
            keyCode_Text = GetComponentInChildren<TextMeshPro>();
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            keyCode_Text.text = "Press Key";
            switch (type)
            {
                case KeyType.Key:
                    SetKey = (key) => InputManager.SetKey(keyName, key);
                    break;
                case KeyType.ValueKey:
                    SetKey = (key) => InputManager.SetValueKey(keyName, key);
                    break;
                case KeyType.AxisPosKey:
                    SetKey = (key) => InputManager.SetAxisPosKey(keyName, key);
                    break;
                case KeyType.AxisNegKey:
                    SetKey = (key) => InputManager.SetAxisNegKey(keyName, key);
                    break;
            }
            InputManager.StartSetKey(SetKey,(key) => keyCode_Text.text = key.ToString());
        }

        public void SetKeyTest(KeyCode keyCode)
        {
            keyCode_Text.text = keyCode.ToString();
        }
    } 
}

