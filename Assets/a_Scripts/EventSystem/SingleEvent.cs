using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.EventSystem
{
    public class SingleEvent : BaseEvent
    {
        private Func<bool> _condition;
        private Action _action;
        
        public SingleEvent(){}
        public SingleEvent(string name) : base(name){}

        public void Register(Func<bool> condition)
        {
            _condition += condition;    
        }

        public void Unregister(Func<bool> condition)
        {
            _condition -= condition;
        }

        public void Subscribe(Action action)
        {
            _action += action;
        }

        public void Unsubscribe(Action action)
        {
            _action -= action;
        }

        public override void Update()
        {
            if(!Enable) return;
            if (_condition != null && _condition())
            {
                _action?.Invoke();
            }
        }
    }
    
    public class SingleEvent<T> : BaseEvent
    {
        private Func<bool> _condition;
        private Func<T> _parameter;
        private Action<T> _action;
        
        public SingleEvent(){}
        public SingleEvent(string name) : base(name){}

        public void Register(Func<bool> condition, Func<T> para)
        {
            _condition += condition;    
            _parameter += para;
        }

        public void Unregister(Func<bool> condition, Func<T> para)
        {
            _condition -= condition;
            _parameter -= para;
        }

        public void Subscribe(Action<T> action)
        {
            _action += action;
        }

        public void Unsubscribe(Action<T> action)
        {
            _action -= action;
        }

        public override void Update()
        {
            if(!Enable) return;
            if (_condition != null && _condition())
            {
                _action?.Invoke(_parameter());
            }
        }
    }
    
    public class SingleEvent<T1, T2> : BaseEvent
    {
        private Func<bool> _condition;
        private Func<T1> _para1;
        private Func<T2> _para2;
        private Action<T1, T2> _action;

        public SingleEvent() { }
        public SingleEvent(string name) : base(name) { }

        public void Register(Func<bool> condition, Func<T1> para1, Func<T2> para2)
        {
            _condition += condition;
            _para1 += para1;
            _para2 += para2;
        }
        public void Unregister(Func<bool> condition, Func<T1> para1, Func<T2> para2)
        {
            _condition -= condition;
            _para1 -= para1;
            _para2 -= para2;
        }

        public void Subscribe(Action<T1, T2> action)
        {
            _action += action;
        }
        public void Unsubscribe(Action<T1, T2> action)
        {
            _action -= action;
        }

        public override void Update()
        {
            if (!Enable) return;
            if (_condition != null && _condition())
            {
                _action?.Invoke(_para1(), _para2());
            }
        }
    }
    
}

