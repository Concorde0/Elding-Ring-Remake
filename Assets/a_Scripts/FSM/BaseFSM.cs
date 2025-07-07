using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace RPG.FSM
{
    public class BaseFSM<T>
    {

        private readonly T _owner;
        private readonly Dictionary<string, FSMState<T>> _states;
        private FSMState<T> _defaultState;
        private FSMState<T> _currentState;
        private FSMState<T> _previousState;

        private bool _isInit = false;
        
        public event Action<string, string> OnStateChanged;

        public BaseFSM(T owner)
        {
            _owner = owner;
            _states = new Dictionary<string, FSMState<T>>();
        }

        public void Update()
        {
            Init();

            if (_currentState != null)
            {
                _currentState.OnUpdate(_owner);
                if (_currentState.CheckCondition(_owner, out string stateName))
                {
                    ChangeState(stateName);
                }
            }
        }

        public void SetDefault(string stateName)
        {
            if (_states.TryGetValue(stateName, out var state))
            {
                _defaultState = state;
                _currentState = _defaultState;
            }
        }

        public void AddState(string stateName, FSMState<T> state)
        {
            if (string.IsNullOrEmpty(stateName) || state == null)
            {
                return;
            }
            _states.Add(stateName, state);
            state.RegisterTransitions(this);
        }

        private void Init()
        {
            if (!_isInit)
            {
                _currentState.OnEnter(_owner);
                _isInit = true;
            }
        }
        
        private string GetStateKey(FSMState<T> state)
        {
            foreach (var pair in _states)
            {
                if (pair.Value == state)
                    return pair.Key;
            }
            return null;
        }

        private void ChangeState(string stateName)
        {
            if (_states.TryGetValue(stateName, out FSMState<T> newState))
            {
                
                string oldState = GetStateKey(_currentState);
                
                _currentState?.OnExit(_owner);
                _currentState = newState;
                OnStateChanged?.Invoke(oldState, stateName);
                _currentState.OnEnter(_owner);
                Debug.Log($"：{oldState} -> {stateName}");
                
            }
        }
        
    }
}


