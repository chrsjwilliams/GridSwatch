using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public void Init()
    {
    }

    public Color GetColor(ColorMode mode, Intensity intensity = Intensity.FULL)
    {
        return _colorScheme.GetColor(mode)[(int)intensity];
    }

    public ColorMode GetDominantColor(Ink inkA, Ink inkB)
    {
        if (inkA.currentLevel == inkB.currentLevel) return ColorMode.NONE;
        return inkA.currentLevel > inkB.currentLevel ? inkA.colorMode : inkB.colorMode;
    }

    public Ink MixColors(Ink inkA, Ink inkB)
    {
        float r = (inkA.color.r + inkB.color.r) / 2f;
        float g = (inkA.color.g + inkB.color.g) / 2f;
        float b = (inkA.color.b + inkB.color.b) / 2f;

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
        else
        {
            cMode = ColorMode.BLACK;
        }

        return new Ink(new Color(r, g, b), cMode, 2);
    }

}
