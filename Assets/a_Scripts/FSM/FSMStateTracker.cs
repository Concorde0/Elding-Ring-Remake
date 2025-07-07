using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.FSM
{
    public class FSMStateTracker
    {
        public string Previous {get; private set;}
        public string Current {get; private set;}

        public void AttachTo<T>(BaseFSM<T> fsm)
        {
            fsm.OnStateChanged += (oldState, newState) =>
            {
                Previous = oldState;
                Current  = newState;
            };
        }
    }
}

