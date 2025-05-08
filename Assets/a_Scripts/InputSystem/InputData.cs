// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using Unity.VisualScripting;
// using LitJson;
// using System.IO;
//
//
// namespace RPG.InputSystem
// {
//     //InputData 是一个 输入系统的数据管理类，用于统一存储、更新和序列化玩家的输入设定
//     [Serializable]
//     public class InputData
//     {
//         public List<Key> keys = new List<Key>();
//         public List<ValueKey> valueKeys = new List<ValueKey>();
//         public List<AxisKey> axisKeys = new List<AxisKey>();
//         public List<ComboKey> comboKeys = new List<ComboKey>();
//
//         public Key GetKeyObject(string name)
//         {
//             return keys.Find(x => x.name == name);
//         }
//         public ValueKey GetValueKeyObject(string name)
//         {
//             return valueKeys.Find(x => x.name == name);
//         }
//         public AxisKey GetAxisKeyObject(string name)
//         {
//             return axisKeys.Find(x => x.name == name);
//         }
//
//         public ComboKey GetComboKeyObject(string name)
//         {
//             return comboKeys.Find(x => x.name == name);
//         }
//
//         public void SetKey(string name, KeyCode keyCode)
//         {
//             Key key = GetKeyObject(name);
//             if (key != null)
//             {
//                 key.SetKey(keyCode);
//             }
//         }
//
//         public void SetValueKey(string name, KeyCode keyCode)
//         {
//             ValueKey valueKey = GetValueKeyObject(name);
//             if (valueKey != null)
//             {
//                 valueKey.SetKey(keyCode);
//             }
//         }
//
//         public void SetAxisKey(string name, KeyCode posKey, KeyCode negKey)
//         {
//             AxisKey axisKey = GetAxisKeyObject(name);
//             if (axisKey != null)
//             {
//                 axisKey.SetKey(posKey, negKey);
//             }
//         }
//
//         public void SetAxisPosKey(string name, KeyCode posKey)
//         {
//             AxisKey axisKey = GetAxisKeyObject(name);
//             if (axisKey != null)
//             {
//                 axisKey.SetPosKey(posKey);
//             }
//         }
//
//         public void SetAxisNegKey(string name, KeyCode negKey)
//         {
//             AxisKey axisKey = GetAxisKeyObject(name);
//             if (axisKey != null)
//             {
//                 axisKey.SetNegKey(negKey);
//             }
//         }
//
//         public void SetComboKey(string name, KeyCode[] keycode)
//         {
//             ComboKey comboKey = GetComboKeyObject(name);
//             if (comboKey != null)
//             {
//                 comboKey.SetKey(keycode);
//             }
//         }
//
//         public bool GetKeyDown(string name)
//         {
//             Key key = GetKeyObject(name);
//             if (key != null)
//             {
//                 return key.isDown;
//             }
//             else
//             {
//                 return false;
//             }
//         }
//
//         public bool GetKeyDownTwice(string name)
//         {
//             Key key = GetKeyObject(name);
//             if (key != null)
//             {
//                 return key.isDoubleDown;
//             }
//             else
//             {
//                 return false;
//             }
//         }
//
//         public float GetValue(string name)
//         {
//            ValueKey valueKey = GetValueKeyObject(name);
//            if (valueKey != null)
//            {
//                return valueKey.value;
//            }
//            else
//            {
//                return 0;
//            }
//         }
//
//         public float GetAxis(string name)
//         {
//             AxisKey axisKey = GetAxisKeyObject(name);
//             if (axisKey != null)
//             {
//                 return axisKey.value;
//             }
//             else
//             {
//                 return 0;
//             }
//         }
//
//         public bool GetComboKeyDown(string name)
//         {
//             ComboKey comboKey = GetComboKeyObject(name);
//             if (comboKey != null)
//             {
//                 return comboKey.isDown;
//             }
//             else
//             {
//                 return false;
//             }
//         }
//
//         public void SetKeyEnable(string name, bool enable)
//         {
//             Key key = GetKeyObject(name);
//             if (key != null)
//             {
//                 key.enable = enable;
//             }
//         }
//
//         public void SetValueKeyEnable(string name, bool enable)
//         {
//             ValueKey valueKey = GetValueKeyObject(name);
//             if (valueKey != null)
//             {
//                 valueKey.enable = enable;
//             }
//         }
//
//         public void SetAxisKeyEnable(string name, bool enable)
//         {
//             AxisKey axisKey = GetAxisKeyObject(name);
//             if (axisKey != null)
//             {
//                 axisKey.enable = enable;
//             }
//         }
//
//         public void SetComboKeyEnable(string name, bool enable)
//         {
//             ComboKey comboKey = GetComboKeyObject(name);
//             if (comboKey != null)
//             {
//                 comboKey.enable = enable;
//             }
//         }
//
//         public void AcceptInput()
//         {
//             UpdateKeys();
//             UpdateValueKeys();
//             UpdateAxisKeys();
//         }
//         private void UpdateKeys()
//         {
//             for (int i = 0; i < keys.Count; i++)
//             {
//                 if (keys[i].enable)
//                 {
//                     keys[i].isDown = false;
//                     keys[i].isDoubleDown = false;
//
//                     switch (keys[i].trigger)
//                     {
//                         case KeyTrigger.Once:
//                             if (Input.GetKeyDown(keys[i].keyCode))
//                             {
//                                 keys[i].isDown = true;
//                             }
//                             break;
//                         case KeyTrigger.Double:
//                             if (keys[i].acceptDoubleDown)
//                             {
//                                 keys[i].realInterval += Time.deltaTime;
//                                 if (keys[i].realInterval > keys[i].pressInterval)
//                                 {
//                                     keys[i].isDoubleDown = false;
//                                     keys[i].realInterval = 0;
//                                 }
//                                 else
//                                 {
//                                     if (Input.GetKeyDown(keys[i].keyCode))
//                                     {
//                                         keys[i].isDoubleDown = true;
//                                         keys[i].realInterval = 0;
//                                     }
//                                     else if(Input.GetKeyUp(keys[i].keyCode))
//                                     {
//                                         keys[i].acceptDoubleDown = false;
//                                     }
//                                 }
//                             }
//                             else
//                             {
//                                 if (Input.GetKeyUp(keys[i].keyCode))
//                                 {
//                                     keys[i].acceptDoubleDown = true;
//                                     keys[i].realInterval = 0;
//                                 }
//                             }
//                             break;
//                         
//                         case KeyTrigger.Continuity:
//                             if (Input.GetKey(keys[i].keyCode))
//                             {
//                                 keys[i].isDown = true;
//                             }
//                             break;
//                     }
//                 }
//             }
//         }
//         private void UpdateValueKeys()
//         {
//             for (int i = 0; i < valueKeys.Count; i++)
//             {
//                 if (valueKeys[i].enable)
//                 {
//                     if (Input.GetKey(valueKeys[i].keyCode))
//                     {
//                         valueKeys[i].value += Mathf.Clamp(valueKeys[i].value + valueKeys[i].addSpeed * Time.deltaTime, valueKeys[i].range.x, valueKeys[i].range.y);
//                     }
//                     else
//                     {
//                         valueKeys[i].value += Mathf.Clamp(valueKeys[i].value - valueKeys[i].addSpeed * Time.deltaTime, valueKeys[i].range.x, valueKeys[i].range.y);
//                     }
//                 }
//             }
//         }
//         private void UpdateAxisKeys()
//         {
//             for (int i = 0; i < axisKeys.Count; i++)
//             {
//                 if (axisKeys[i].enable)
//                 {
//                     if (Input.GetKey(axisKeys[i].posKey))
//                     {
//                         axisKeys[i].value = Mathf.Clamp(axisKeys[i].value + axisKeys[i].addSpeed * Time.deltaTime, axisKeys[i].range.x, axisKeys[i].range.y);
//                     }
//                     else if(Input.GetKey(axisKeys[i].negKey))
//                     {
//                         axisKeys[i].value = Mathf.Clamp(axisKeys[i].value - axisKeys[i].addSpeed * Time.deltaTime, axisKeys[i].range.x, axisKeys[i].range.y);
//                     }
//                     else
//                     {
//                         axisKeys[i].value = Mathf.Lerp(axisKeys[i].value, 0, axisKeys[i].addSpeed * Time.deltaTime);
//                         if (Mathf.Abs(axisKeys[i].value - axisKeys[i].addSpeed * Time.deltaTime) < 0.01f)
//                         {
//                             axisKeys[i].value = 0;
//                         }
//                     }
//                 }
//             }
//         }
//
//         private void UpdateComboKeys()
//         {
//             for (int i = 0; i < comboKeys.Count; i++)
//             {
//                 if (comboKeys[i].enable)
//                 {
//                     bool allKeysPressed = true;
//
//                     // 检查绑定的所有按键是否按下
//                     foreach (KeyCode key in comboKeys[i].keyCodes)
//                     {
//                         if (!Input.GetKey(key))
//                         {
//                             allKeysPressed = false;
//                             break;  // 如果有任意一个按键没有按下，就认为组合键未激活
//                         }
//                     }
//
//                     // 如果所有按键都按下，标记组合键为按下
//                     if (allKeysPressed)
//                     {
//                         comboKeys[i].isDown = true;
//                     }
//                     else
//                     {
//                         comboKeys[i].isDown = false;
//                     }
//                 }
//             }
//         }
//
//         public void SaveInputSetting(string path)
//         {
//             JsonData json = new JsonData();
//             json["Keys"] = new JsonData();
//             foreach (Key key in keys)
//             {
//                 json["Keys"][key.name] = key.keyCode.ToString();
//             }
//             
//             json["ValueKeys"] = new JsonData();
//             foreach (ValueKey valueKey in valueKeys)
//             {
//                 json["ValueKeys"][valueKey.name] = valueKey.keyCode.ToString();
//             }
//             
//             json["AxisKeys"] = new JsonData();
//             foreach (AxisKey axisKey in axisKeys)
//             {
//                 json["AxisKeys"][axisKey.name] = new JsonData();
//                 json["AxisKeys"][axisKey.name]["Pos"] = axisKey.posKey.ToString();
//                 json["AxisKeys"][axisKey.name]["Neg"] = axisKey.negKey.ToString();
//             }
//             
//             string filePath = Application.dataPath + path;
//             FileInfo file = new FileInfo(filePath);
//             StreamWriter sw = file.CreateText();
//             sw.WriteLine(json.ToJson());
//             sw.Close();
//             sw.Dispose();
//         }
//
//         public void LoadInputSetting(string path)
//         {
//             string filePath = Application.dataPath + path;
//             if (!File.Exists(filePath))
//             {
//                 return;
//             }
//             
//             string data = File.ReadAllText(filePath);
//             JsonData json = JsonMapper.ToObject(data);
//             foreach (Key key in keys)
//             {
//                 key.keyCode = StringToEnum_KeyCode(json["Keys"][key.name].ToString());
//             }
//
//             foreach (ValueKey valueKey in valueKeys)
//             {
//                 valueKey.keyCode = StringToEnum_KeyCode(json["ValueKeys"][valueKey.name].ToString());
//             }
//
//             foreach (AxisKey axisKey in axisKeys)
//             {
//                 axisKey.posKey = StringToEnum_KeyCode(json["AxisKeys"][axisKey.name].ToString());
//                 axisKey.negKey = StringToEnum_KeyCode(json["AxisKeys"][axisKey.name].ToString());
//             }
//         }
//         private KeyCode StringToEnum_KeyCode(string key)
//         {
//             return (KeyCode)Enum.Parse(typeof(KeyCode), key);
//         }
//     }
// }
//
