using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace RPG.FSM
{
    //单参数委托
    public class FSMCondition<T>
    {
        // ConditionHandler<T> 是一个类型，它代表接收一个 T、返回 bool的方法
        private Func<T, bool> _conditionHandle;
        
        public FSMCondition() { }
        public FSMCondition(Func<T, bool> handle)
        {
            BindCondition(handle);
        }

        public void BindCondition(Func<T, bool> handle)
        {
            _conditionHandle = handle;
        }

        public virtual bool Condition(T owner)
        {
            return _conditionHandle != null && _conditionHandle.Invoke(owner);
        }
        

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
    public class FSMCondition<T1, T2> : FSMCondition<T1>
    {
        private readonly Func<T1, T2, bool> _condition;
        private readonly T2 _value;
        public FSMCondition() { }
        public FSMCondition(Func<T1, T2, bool> condition, T2 value)
        {
            _condition = condition;
            _value = value;
        }

        public override bool Condition(T1 owner)
        {
            return _condition != null && _condition(owner, _value);
        }
    }
}

