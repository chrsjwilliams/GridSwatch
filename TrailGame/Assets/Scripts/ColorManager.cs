using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorMode { NONE = 0, CYAN, MAGENTA, YELLOW, BLACK, GREEN, PURPLE, ORANGE }

public class ColorManager : MonoBehaviour
{
    public enum Intensity { FULL = 0, DIM }

    public List<Color[]> Colors;

    public Color ErrorColor;

    public Color[] Magenta;
    public Color[] Cyan;
    public Color[] Yellow;
    public Color[] Black;

    public Color[] Purple;
    public Color[] Orange;
    public Color[] Green;

    public void Init()
    {
        Colors = new List<Color[]>();
        Colors.Add(Cyan);
        Colors.Add(Magenta);
        Colors.Add(Yellow);
        Colors.Add(Black);
    }

    public Color GetColor(ColorMode mode, Intensity intensity = Intensity.FULL)
    {
        switch (mode)
        {
            case ColorMode.MAGENTA:
                return Magenta[(int)intensity];
            case ColorMode.YELLOW:
                return Yellow[(int)intensity];
            case ColorMode.CYAN:
                return Cyan[(int)intensity];
            case ColorMode.GREEN:
                return Green[(int)intensity];
            case ColorMode.ORANGE:
                return Orange[(int)intensity];
            case ColorMode.PURPLE:
                return Purple[(int)intensity];
            case ColorMode.BLACK:
                return Black[(int)intensity];
            default:
                return Black[(int)Intensity.FULL];
        }
    }

    public ColorMode GetDominantColor(Ink inkA, Ink inkB)
    {
        if (inkA.currentLevel == inkB.currentLevel) return ColorMode.NONE;
        return inkA.currentLevel > inkB.currentLevel ? inkA.colorMode : inkB.colorMode;
    }

    public Ink MixColors(Ink inkA, Ink inkB)
    {
        Debug.Log("INK A: " + inkA.colorMode + " | INK B: " + inkB.colorMode);
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
