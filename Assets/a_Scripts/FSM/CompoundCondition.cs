using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompoundCondition<T>
{
    private List<Func<T, bool>> _enterConditions = new List<Func<T, bool>>();
    private List<Func<T, bool>> _exitConditions = new List<Func<T, bool>>();

    public CompoundCondition<T> AddEnterCondition(Func<T, bool> condition)
    {
        _enterConditions.Add(condition);
        return this;
    }

    public CompoundCondition<T> AddExitCondition(Func<T, bool> condition)
    {
        _exitConditions.Add(condition);
        return this;
    }

    // 生成最终用于 FSMCondition 的复合条件
    public Func<T, bool> Build()
    {
        return (T target) =>
        {
            // 检查是否满足所有进入条件
            bool enter = _enterConditions.All(c => c(target));
            // 检查是否满足任何退出条件
            bool exit = _exitConditions.Any(c => c(target));
            return enter && !exit;
        };
    }
}
