using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


namespace RPG.EventSystem
{
    public abstract class BaseEvent
    {
        public string Name { get ; protected set ; }
        public bool Enable { get ; protected set ; }
        
        public BaseEvent(){}
        public BaseEvent(string name)
        {
            Name = name;
        }

        public virtual void SetName(string name)
        {
            Name = name;
        }

        public virtual void SetEnable(bool enable,bool includeChildren = true)
        {
            Enable = enable;
        }
        
        public virtual BaseEvent Find(string name)
        {
            return Name == name ? this : null;
        }


        public virtual void Update(){ }
    }
}

