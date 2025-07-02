using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TriggerParam : MonoBehaviour
{
    private float _triggerTime = -1f;
    private readonly float _validDuration;
    public TriggerParam(float validDuration = 0.05f)
    {
        _validDuration = validDuration;
    }

    public void Set()
    {
        _triggerTime = Time.time;
    }

    public bool Consume()
    {
        if (_triggerTime < 0f) return false;

        if (Time.time - _triggerTime <= _validDuration)
        {
            _triggerTime = -1f;
            return true;
        }

        _triggerTime = -1f;
        return false;
    }

    public bool Peek()
    {
        return _triggerTime >= 0f && (Time.time - _triggerTime <= _validDuration);
    }
}
