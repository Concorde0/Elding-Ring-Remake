using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.FSM
{
    public class StateCache
    {
        private string _cachedState = null;
        public bool HasCached => !string.IsNullOrEmpty(_cachedState);

        public void Cache(string stateName)
        {
            _cachedState = stateName;
        }

        public string Consume()
        {
            string result = _cachedState;
            _cachedState = null;
            return result;
        }

        public void Clear()
        {
            _cachedState = null;
        }
        
        public string Peek() => _cachedState;
    }
}

