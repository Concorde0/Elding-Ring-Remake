// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
//
//
// namespace RPG.InputSystem
// {
//     public class InputManager
//     {
//         private static string _DefaultDataSavePath = "/Resources/DefaultDataInputData.json";
//         private static string _CustomDataSavePath = "/Resources/CustomDataInputData.json";
//         private static InputData _inputData;
//         private static bool _activeInput;
//         private static Action<KeyCode> _setKeyHandler;
//         private static Action<KeyCode> _displayKeyHandler;
//
//         public InputManager(InputData inputData)
//         {
//             _inputData = inputData;
//             SaveDefaultSetting();
//             LoadCustomSetting();
//         }
//
//         public void Update()
//         {
//             _inputData.AcceptInput();
//             if (_activeInput)
//             {
//                 foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
//                 {
//                     if (Input.GetKeyDown(keyCode))
//                     {
//                         if (_setKeyHandler != null)
//                         {
//                             _setKeyHandler(keyCode);
//                         }
//
//                         if (_displayKeyHandler != null)
//                         {
//                             _displayKeyHandler(keyCode);
//                         }
//
//                         _activeInput = false;
//                         _setKeyHandler = null;
//                         _displayKeyHandler = null;
//                     }
//                 }
//             }
//         }
//
//         public static bool GetKey(string name)
//         {
//             return _inputData.GetKeyDown(name);
//         }
//
//         public static bool GetKeyDown(string name)
//         {
//             return _inputData.GetKeyDown(name);
//         }
//
//         public static bool GetKeyTwice(string name)
//         {
//             return _inputData.GetKeyDownTwice(name);
//         }
//
//         public static float GetValue(string name)
//         {
//             return _inputData.GetValue(name);
//         }
//
//         public static float GetAxis(string name)
//         {
//             return _inputData.GetAxis(name);
//         }
//
//         public static bool GetCombo(string name)
//         {
//             return _inputData.GetComboKeyDown(name);
//         }
//
//         public static void SetKey(string name, KeyCode keyCode)
//         {
//             _inputData.SetKey(name, keyCode);
//         }
//
//         public static void SetValueKey(string name, KeyCode keyCode)
//         {
//             _inputData.SetValueKey(name, keyCode);
//         }
//
//         public static void SetAxis(string name,KeyCode pos,KeyCode neg)
//         {
//             _inputData.SetAxisKey(name, pos,neg);
//         }
//
//         public static void SetAxisPosKey(string name, KeyCode pos)
//         {
//             _inputData.SetAxisPosKey(name, pos);
//         }
//
//         public static void SetAxisNegKey(string name, KeyCode neg)
//         {
//             _inputData.SetAxisNegKey(name, neg);
//         }
//
//         public static void SetComboKey(string name, KeyCode[] combo)
//         {
//             _inputData.SetComboKey(name, combo);
//         }
//
//         public static void StartSetKey(Action<KeyCode> setKey, Action<KeyCode> displayKey)
//         {
//             _activeInput = true;
//             _setKeyHandler = setKey;
//             _displayKeyHandler = displayKey;
//         }
//
//         public static void SaveDefaultSetting()
//         {
//             _inputData.SaveInputSetting(_DefaultDataSavePath);
//         }
//
//         public static void LoadDefaultSetting()
//         {
//             _inputData.LoadInputSetting(_DefaultDataSavePath);
//         }
//
//         public static void SaveCustomSetting()
//         {
//             _inputData.SaveInputSetting(_CustomDataSavePath);
//         }
//
//         public static void LoadCustomSetting()
//         {
//             _inputData.LoadInputSetting(_CustomDataSavePath);
//         }
//
//         public static KeyCode GetKeyCode(string name)
//         {
//             Key key = _inputData.GetKeyObject(name);
//             if (key != null)
//             {
//                 return key.keyCode;
//             }
//             return KeyCode.None;
//         }
//         
//         public static KeyCode GetValueKeyCode(string name)
//         {
//             ValueKey valueKey = _inputData.GetValueKeyObject(name);
//             if (valueKey != null)
//             {
//                 return valueKey.keyCode;
//             }
//             return KeyCode.None;
//         }
//         
//         public static KeyCode GetAxisPosCode(string name)
//         {
//             AxisKey axisKey = _inputData.GetAxisKeyObject(name);
//             if (axisKey != null)
//             {
//                 return axisKey.posKey;
//             }
//             return KeyCode.None;
//         }
//         
//         public static KeyCode GetAxisNegKeyCode(string name)
//         {
//             AxisKey axisKey = _inputData.GetAxisKeyObject(name);
//             if (axisKey != null)
//             {
//                 return axisKey.negKey;
//             }
//             return KeyCode.None;
//         }
//
//         public static KeyCode[] GetComboKey(string name)
//         {
//             ComboKey comboKey = _inputData.GetComboKeyObject(name);
//             if (comboKey != null)
//             {
//                 return comboKey.keyCodes;
//             }
//             return new[] { KeyCode.None };
//         }
//         
//     }
// }
//
