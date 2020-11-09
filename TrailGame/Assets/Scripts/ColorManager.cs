using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
