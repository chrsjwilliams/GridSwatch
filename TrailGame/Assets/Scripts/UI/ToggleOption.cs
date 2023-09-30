using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class ToggleOption : Option
{
    [SerializeField] MonoTweener onTweens;
    [SerializeField] MonoTweener offTweens;

    [SerializeField, ReadOnly] bool _isOn;
    public bool IsOn { get { return _isOn; } }

    public Action OnToggleOn;
    public Action OnToggleOff;

    public void InitOption(bool defaultStatus, Action onAction, Action offAction)
    {
        _isOn = defaultStatus;

        OnToggleOn = onAction;
        OnToggleOff = offAction;

        if (_isOn)
        {
            onTweens?.Play(() => { OnToggleOn?.Invoke(); });
        }
        else
        {
            offTweens?.Play(() => { OnToggleOff?.Invoke(); });
        }
    }

    public void Toggle()
    {
        _isOn = !_isOn;
        if(_isOn)
        {
            onTweens?.Play(() => { OnToggleOn?.Invoke(); });
        }
        else
        {
            offTweens?.Play(() => { OnToggleOff?.Invoke(); });
        }
    }

}
