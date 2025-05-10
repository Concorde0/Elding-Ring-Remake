using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.FSM
{
    public class BaseFSM<T>
    {
        public string currentStateName;

        private readonly T _owner;
        private readonly Dictionary<string, FSMState<T>> _states;
        
        private FSMState<T> _defaultState;
        private FSMState<T> _currentState;

        private bool _isInit = false;

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
                    currentStateName = stateName;
                }
            }
        }

        public void SetDefault(string stateName)
        {
            if (_states.ContainsKey(stateName))
            {
                _defaultState = _states[stateName];
                _currentState = _defaultState;
                currentStateName = stateName;
            }
        }

        public void AddState(string stateName, FSMState<T> state)
        {
            if (string.IsNullOrEmpty(stateName) || state == null)
            {
                return;
            }
            _states.Add(stateName, state);
        }

        private void Init()
        {
            if (!_isInit)
            {
                _currentState.OnEnter(_owner);
                _isInit = true;
            }
        }

        private void ChangeState(string stateName)
        {
            if (_states.TryGetValue(stateName, out FSMState<T> state))
            {
                _currentState.OnExit(_owner);
                state.OnEnter(_owner);
                _currentState = state;
            }
        }
    }
}


