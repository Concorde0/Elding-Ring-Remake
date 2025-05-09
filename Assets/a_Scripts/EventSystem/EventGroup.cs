using System.Collections;
using System.Collections.Generic;
using RPG.EventSystem;
using UnityEngine;


namespace RPG.EventSystem
{
    public class EventGroup : BaseEvent
    {
        private List<BaseEvent> _children;
        
        public EventGroup(){}

        public EventGroup(string name) : base(name) { }

        public void AddEvent(BaseEvent e)
        {
            if(e == null) return;
            _children ??= new List<BaseEvent>();
            _children.Add(e);
        }

        public bool RemoveEvent(BaseEvent e)
        {
            return _children != null && _children.Remove(e);
        }

        public override void SetEnable(bool enable, bool includeChildren = true)
        {
            base.SetEnable(enable);
            if(!includeChildren)return;
            foreach (var e in _children)
            {
                e.SetEnable(enable);
            }
        }

        public override BaseEvent Find(string name)
        {
            if(Name == name) return this;
            BaseEvent target = null;
            foreach (var e in _children)
            {
                target = e.Find(name);
                if(target != null) break;
            }
            return target;
        }

        public override void Update()
        {
            if(!Enable) return;
            foreach (var e in _children)
            {
                e.Update();
            }
        }
    }
}

