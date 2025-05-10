using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace RPG.FSM
{
    //单参数委托
    public abstract class FSMCondition<T>
    {
        public delegate bool ConditionHandler<T>(T owner);
        private ConditionHandler<T> _conditionHandler;
        
        public FSMCondition() { }
        public FSMCondition(ConditionHandler<T> handle)
        {
            BindCondition(handle);
        }

        public void BindCondition(ConditionHandler<T> handle)
        {
            _conditionHandler = handle;
        }

        public virtual bool Condition(T owner)
        {
            return _conditionHandler != null && _conditionHandler.Invoke(owner);
        }
        
        //运算符重载 ！ | &

        public static FSMCondition<T> operator &(FSMCondition<T> condition1, FSMCondition<T> condition2)
        {
            return new AndCondition<T>(condition1, condition2);
        }

        public static FSMCondition<T> operator |(FSMCondition<T> condition1, FSMCondition<T> condition2)
        {
            return new OrCondition<T>(condition1, condition2);
        }

        public static FSMCondition<T> operator !(FSMCondition<T> condition)
        {
            return new NotCondition<T>(condition);
        }
        
    }
    
    //双参数委托
    public abstract class FSMCondition<T1, T2> : FSMCondition<T1>
    {
        public delegate bool ConditionHandler<T1,T2>(T1 owner1,T2 owner2);
        private readonly ConditionHandler<T1, T2> _condition;
        private readonly T2 _value;
        public FSMCondition() { }

        public FSMCondition(ConditionHandler<T1, T2> handle, T2 value)
        {
            _condition = handle;
            _value = value;
        }

        public override bool Condition(T1 owner)
        {
            return _condition != null && _condition(owner, _value);
        }
    }
}

