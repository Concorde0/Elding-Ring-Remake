using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.FSM
{
    public class FSMState<T>
    {
        private Dictionary<FSMCondition<T>, string> _conditionMaps;
        
        private Action<T> _enterHandle;
        private Action<T> _updateHandle;
        private Action<T> _exitHandle;

        public void BindEnterAction(Action<T> action)
        {
            _enterHandle = action;
        }

        public void BindUpdateAction(Action<T> action)
        {
            _updateHandle = action;
        }

        public void BindExitAction(Action<T> action)
        {
            _exitHandle = action;
        }

        public void AddCondition(FSMCondition<T> condition, string stateName)
        {
            if (condition == null || string.IsNullOrEmpty(stateName))
            {
                return;
            }

            if (_conditionMaps == null)
            {
                _conditionMaps = new Dictionary<FSMCondition<T>, string>();
            }
            _conditionMaps.Add(condition, stateName);
        }

        public void RemoveCondition(FSMCondition<T> condition)
        {
            if (_conditionMaps == null || condition == null)
            {
                return;
            }
            _conditionMaps.Remove(condition);
        }

        public bool CheckCondition(T owner, out string stateName)
        {
            if (_conditionMaps == null)
            {
                stateName = string.Empty;
                return false;
            }

            foreach (var condition in _conditionMaps.Keys)
            {
                if (condition.Condition(owner))
                {
                    stateName = _conditionMaps[condition];
                    return true;
                }
            }
            stateName = string.Empty;
            return false;
        }

        public virtual void OnEnter(T owner)
        {
            if (_enterHandle != null)
            {
                _enterHandle(owner);
            }
        }

        public virtual void OnUpdate(T owner)
        {
            if (_updateHandle != null)
            {
                _updateHandle(owner);
            }
        }

        public virtual void OnExit(T owner)
        {
            if (_exitHandle != null)
            {
                _exitHandle(owner);
            }
        }
    }
}


