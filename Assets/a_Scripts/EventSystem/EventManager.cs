using System;

namespace RPG.EventSystem
{
    public class EventManager : Singleton<EventManager>
    {
        private EventGroup _root = new EventGroup("Root");

        public SingleEvent CreateEvent(string name, string parent = "Root")
        {
            return Create<SingleEvent>(name, parent);
        }
        
        public SingleEvent<T> CreatEvent<T>(string name, string parent = "Root")
        {
            return Create<SingleEvent<T>>(name, parent);
        }
        
        public SingleEvent<T1, T2> CreateEvent<T1, T2>(string name, string parent = "Root")
        {
            return Create<SingleEvent<T1, T2>>(name, parent);
        }
        
        public EventGroup CreateEventGroup(string name, string parent = "Root")
        {
            return Create<EventGroup>(name,parent);
        }
        
        public bool Register(string name, Func<bool> condition)
        {
            if(!(_root.Find(name) is SingleEvent e)) return false;
            e.Register(condition);
            return true;
        }

        public bool Register<T>(string name, Func<bool> condition, Func<T> para)
        {
            if(!(_root.Find(name) is SingleEvent<T> e)) return false;
            e.Register(condition,para);
            return true;
        }

        public bool Register<T1, T2>(string name, Func<bool> condition, Func<T1> para1, Func<T2> para2)
        {
            if(!(_root.Find(name) is SingleEvent<T1, T2> e)) return false;
            e.Register(condition,para1,para2);
            return true;
        }
        
        public bool UnRegister(string name, Func<bool> condition)
        {
            if(!(_root.Find(name) is SingleEvent e)) return false;
            e.Unregister(condition);
            return true;
        }

        public bool UnRegister<T>(string name, Func<bool> condition, Func<T> para)
        {
            if(!(_root.Find(name) is SingleEvent<T> e)) return false;
            e.Unregister(condition,para);
            return true;
        }

        public bool UnRegister<T1, T2>(string name, Func<bool> condition, Func<T1> para1, Func<T2> para2)
        {
            if(!(_root.Find(name) is SingleEvent<T1, T2> e)) return false;
            e.Unregister(condition,para1,para2);
            return true;
        }
        
        public bool Subscribe(string name, Action action)
        {
            if(!(_root.Find(name) is SingleEvent e)) return false;
            e.Subscribe(action);
            return true;
        }

        public bool Subscribe<T>(string name, Action<T> action)
        {
            if(!(_root.Find(name) is SingleEvent<T> e)) return false;
            e.Subscribe(action);
            return true;
        }

        public bool Subscribe<T1, T2>(string name, Action<T1, T2> action)
        {
            if(!(_root.Find(name) is SingleEvent<T1, T2> e)) return false;
            e.Subscribe(action);
            return true;
        }
        
        public bool UnSubscribe(string name, Action action)
        {
            if(!(_root.Find(name) is SingleEvent e)) return false;
            e.Unsubscribe(action);
            return true;
        }

        public bool UnSubscribe<T>(string name, Action<T> action)
        {
            if(!(_root.Find(name) is SingleEvent<T> e)) return false;
            e.Unsubscribe(action);
            return true;
        }

        public bool UnSubscribe<T1, T2>(string name, Action<T1, T2> action)
        {
            if(!(_root.Find(name) is SingleEvent<T1, T2> e)) return false;
            e.Unsubscribe(action);
            return true;
        }
        
        
        
        public void Enable(string name, bool enable, bool includeChildren = true)
        {
            var target = _root.Find(name);
            if(target == null) return;
            target.SetEnable(enable, includeChildren);
        }
        public void Update()
        {
            _root?.Update();
        }
        
        private T Create<T>(string name, string parent) where T : BaseEvent, new()
        {
            if (!(_root.Find(parent) is EventGroup group)) return null;
            var e = new T();
            e.SetName(name);
            group.AddEvent(e);
            return e;
        }
    }
}

