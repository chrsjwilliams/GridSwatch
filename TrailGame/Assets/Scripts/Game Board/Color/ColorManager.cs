using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static ColorManager;

public enum ColorMode { NONE = 0, CYAN, MAGENTA, YELLOW, BLACK, GREEN, PURPLE, ORANGE }

public class ColorManager : MonoBehaviour
{
    public enum Intensity { FULL = 0, DIM }

    [SerializeField] Color dayModeBackgroundColor;
    [SerializeField] Color nightModeBackgroundColor;

    [SerializeField] ColorSchemeOption _colorScheme;
    public ColorSchemeOption ColorScheme { get { return _colorScheme; } }

    [SerializeField] ColorSchemeOption defaultColorScheme;
    [SerializeField] ColorSchemeOption _colorBlindScheme;

    private void OnEnable()
    {
        DayNightModeButton.DisplayModeChanged += OnDisplayModeChanged;
    }

    private void OnDisable()
    {
        DayNightModeButton.DisplayModeChanged -= OnDisplayModeChanged;
    }

    private void Awake()
    {
        _colorScheme = defaultColorScheme;
    }

    private void OnDisplayModeChanged(DayNightModeButton.DisplayMode mode)
    {
        if (mode == DayNightModeButton.DisplayMode.DAY)
        {
            Camera.main.DOColor(dayModeBackgroundColor, 0.33f).SetEase(Ease.InOutQuint);
        }
        else
        {
            Camera.main.DOColor(nightModeBackgroundColor, 0.33f).SetEase(Ease.InOutQuint);
        }

    }

    public Color GetColor(ColorMode mode, Intensity intensity = Intensity.FULL)
    {
        return _colorScheme.GetColor(mode)[(int)intensity];
    }

    public Sprite GetColorblindPattern(ColorMode mode)
    {
        return _colorScheme.GetColorblindPattern(mode);
    }

    public ColorMode GetDominantColor(Ink inkA, Ink inkB)
    {
        if (inkA.currentLevel == inkB.currentLevel) return ColorMode.NONE;
        return inkA.currentLevel > inkB.currentLevel ? inkA.colorMode : inkB.colorMode;
    }

    public Ink MixColors(Ink inkA, Ink inkB)
    {
        ColorMode cMode = ColorMode.NONE;
        if(inkA.colorMode == ColorMode.MAGENTA && inkB.colorMode == ColorMode.CYAN ||
            inkA.colorMode == ColorMode.CYAN && inkB.colorMode == ColorMode.MAGENTA)
        {
            cMode = ColorMode.PURPLE;
        }
        else if (inkA.colorMode == ColorMode.MAGENTA && inkB.colorMode == ColorMode.YELLOW ||
            inkA.colorMode == ColorMode.YELLOW && inkB.colorMode == ColorMode.MAGENTA)
        {
            cMode = ColorMode.ORANGE;
        }
        else if (inkA.colorMode == ColorMode.CYAN && inkB.colorMode == ColorMode.YELLOW ||
            inkA.colorMode == ColorMode.YELLOW && inkB.colorMode == ColorMode.CYAN)
        {
            cMode = ColorMode.GREEN;
        }
        else if (((inkA.colorMode == ColorMode.CYAN || inkA.colorMode == ColorMode.MAGENTA) && inkB.colorMode == ColorMode.PURPLE) ||
            ((inkB.colorMode == ColorMode.CYAN || inkB.colorMode == ColorMode.MAGENTA) && inkA.colorMode == ColorMode.PURPLE))
        {
            cMode = ColorMode.PURPLE;
        }
        else if (((inkA.colorMode == ColorMode.CYAN || inkA.colorMode == ColorMode.YELLOW) && inkB.colorMode == ColorMode.GREEN) ||
            ((inkB.colorMode == ColorMode.CYAN || inkB.colorMode == ColorMode.YELLOW) && inkA.colorMode == ColorMode.GREEN))
        {
            cMode = ColorMode.GREEN;
        }
        else if (((inkA.colorMode == ColorMode.YELLOW || inkA.colorMode == ColorMode.MAGENTA) && inkB.colorMode == ColorMode.ORANGE) ||
            ((inkB.colorMode == ColorMode.YELLOW || inkB.colorMode == ColorMode.MAGENTA) && inkA.colorMode == ColorMode.ORANGE))
        {
            cMode = ColorMode.ORANGE;
        }
        else
        {
            cMode = ColorMode.BLACK;
        }

        int intensity = 0;
        
        return new Ink(ColorScheme.GetColor(cMode)[intensity], cMode, intensity + 1);
    }

}
