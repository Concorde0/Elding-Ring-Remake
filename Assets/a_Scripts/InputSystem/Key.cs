using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.InputSystem
{
    //TODO:缺少组合键
    public enum KeyTrigger{
        Once,Double,Continuity,
    }
    
    //控制单击或双击
    
    [Serializable]
    public class Key
    {
        
        public string name;
        public KeyTrigger trigger;
        [HideInInspector] public bool isDown;
        [HideInInspector] public bool isDoubleDown;
        [HideInInspector] public bool acceptDoubleDown;

        //检测双击之间的间隔
        public float pressInterval = 1f;
        [HideInInspector] public float realInterval;
        public KeyCode keyCode;
        [HideInInspector] public bool enable = true;

        public void SetKey(KeyCode key)
        {
            keyCode = key;
        }

        public void SetEnable(bool enable)
        {
            this.enable = enable;
            isDown = false;
            isDoubleDown = false;
        }
    }


    [Serializable]
        public class ValueKey
        {
            public string name;
            public Vector2 range = new Vector2(0f, 1f);
            [HideInInspector]
            public float value;
            public float addSpeed = 3f;
            public KeyCode keyCode;
            [HideInInspector]
            public bool enable;
            
            public void SetKey(KeyCode key)
            {
                keyCode = key;
            }

            public void SetEnable(bool enable)
            {
                this.enable = enable;
                value = 0f;
            }

        }
        
        
        [Serializable]
        public class AxisKey
        {
            public string name;
            public Vector2 range = new Vector2(-1f, 1f);
            [HideInInspector]
            public float value;
            public float addSpeed = 3f;
            public KeyCode posKey;
            public KeyCode negKey;
            [HideInInspector]
            public bool enable;

            public void SetKey(KeyCode pos, KeyCode neg)
            {
                posKey = pos;
                negKey = neg;
                
            }

            public void SetPosKey(KeyCode pos)
            {
                posKey = pos;
            }

            public void SetNegKey(KeyCode neg)
            {
                negKey = neg;
            }

            public void SetEnable(bool enable)
            {
                this.enable = enable;
                value = 0;
            }
        }

        [Serializable]
        public class ComboKey
        {
            public string name;
            public KeyCode[] keyCodes;
            public bool isDown;
            public bool enable = true;

            public void SetKey(KeyCode[] keys)
            {
                keyCodes = keys;
            }

            public bool IsPressedAllKeys(KeyCode[] keys)
            {
                if (!enable || keyCodes == null || keyCodes.Length == 0) return false;

                foreach (var key in keyCodes)
                {
                    if (!Input.GetKey(key)) return false;
                }
                return true;
            }

            public void SetEnable(bool enable)
            {
                this.enable = enable;
                isDown = false;
            }
        }
        
    }


