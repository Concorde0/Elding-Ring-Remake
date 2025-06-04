using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.FSM
{
    public class BaseFSM<T>
    {

        private readonly T _owner;
        private readonly Dictionary<string, FSMState<T>> _states;
        public StateTracker Tracker { get; private set; } = new StateTracker();
        public StateCache Cache { get; private set; } = new StateCache();
        
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

        private void ChangeState(string stateName)
        {
            if (_states.TryGetValue(stateName, out FSMState<T> newState))
            {
                _currentState?.OnExit(_owner);
                _currentState = newState;
                _currentState.OnEnter(_owner);
                Tracker.SetState(stateName);
            }
        }
    }
}


