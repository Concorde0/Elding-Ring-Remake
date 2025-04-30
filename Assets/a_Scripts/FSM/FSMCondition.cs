using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace RPG.FSM
{
    public abstract class FSMCondition<T>
    {
        private Func<T, bool> _conditionHandle;
        
        public FSMCondition() { }
        
    }
}

