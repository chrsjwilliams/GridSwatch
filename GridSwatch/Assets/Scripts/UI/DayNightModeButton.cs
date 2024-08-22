using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class DayNightModeButton : Option
{
    public static DisplayMode CurrentDisplayMode;
    public enum DisplayMode { NONE = 0, DAY, NIGHT}

    [SerializeField, ReadOnly] private DisplayMode _currentMode;

    const string displayModeKey = "DISPLAY_MODE";

    [Space(25)]
    [SerializeField] MonoTweener dayModeOnTweens;
    [SerializeField] MonoTweener dayModeOffTweens;
    [SerializeField] Sprite dayOnIcon;
    [SerializeField] Sprite dayOffIcon;
    [SerializeField] Image dayIcon; 

    [Space(25)]
    [SerializeField] MonoTweener nightModeOnTweens;
    [SerializeField] MonoTweener nightModeOffTweens;
    [SerializeField] Sprite nightOnIcon;
    [SerializeField] Sprite nightOffIcon;
    [SerializeField] Image nightIcon;



    public static Action<DisplayMode> DisplayModeChanged;

    private void Awake()
    {
        _currentMode = DisplayMode.NIGHT;
        if (PlayerPrefs.HasKey(displayModeKey))
        {
            _currentMode = (DisplayMode)PlayerPrefs.GetInt(displayModeKey);
        }

        ToggleDisplayTo((int)_currentMode);
    }

    public void ToggleDisplayTo(int mode)
    {
        if(mode < 1 || mode > 2)
        {
            Debug.LogError("Error Setting Day Mode or Night Mode. Mode should eight be 1 for DAY or 2 for NIGHT. Please check your Day Night Buttons");
            return;
        }

        _currentMode = (DisplayMode)mode;

        if(_currentMode == DisplayMode.DAY)
        {
            dayModeOnTweens?.Play();
            nightModeOffTweens?.Play();
        }
        else
        {
            nightModeOnTweens?.Play();
            dayModeOffTweens?.Play();
        }

        dayIcon.sprite = _currentMode == DisplayMode.DAY ? dayOnIcon : dayOffIcon;
        nightIcon.sprite = _currentMode == DisplayMode.NIGHT ? nightOnIcon : nightOffIcon;

        CurrentDisplayMode = _currentMode;

        DisplayModeChanged?.Invoke(_currentMode);
        PlayerPrefs.SetInt(displayModeKey, (int)mode);
        PlayerPrefs.Save();
    }
}
