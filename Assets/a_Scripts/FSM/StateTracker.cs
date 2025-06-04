using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.FSM
{
    public class StateTracker
    {
        public string currentState { get; private set; }
        public string previousState { get; private set; }

        public void SetState(string newState)
        {
            if (newState == currentState) return;
            previousState = currentState;
            currentState = newState;
        }
    }
}


