using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public enum Intensity { FULL = 0, DIM }

    public List<Color[]> Colors;

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
        Intensity intensityIndexA = inkA.Intensity > 1 ? Intensity.FULL : Intensity.DIM;
        Intensity intensityIndexB = inkB.Intensity > 1 ? Intensity.FULL : Intensity.DIM;

        ColorMode dominantColor = GetDominantColor(inkA, inkB);
        
        if (inkA.currentLevel > inkB.currentLevel)
        {
            // Turn this into a function
            switch(inkA.colorMode)
            {
                case ColorMode.MAGENTA:
                    if(inkB.colorMode == ColorMode.CYAN)
                    {
                        Debug.Log("M-PURPLE");
                        return new Ink (Purple[2], ColorMode.PURPLE, 2);
                    }
                    else if (inkB.colorMode == ColorMode.YELLOW)
                    {
                        Debug.Log("M-ORANGE");
                        return new Ink (Orange[2], ColorMode.ORANGE, 2);
                    }
                    break;
                case ColorMode.CYAN:
                    if (inkB.colorMode == ColorMode.MAGENTA)
                    {
                        Debug.Log("C-PURPLE");
                        return new Ink(Purple[3], ColorMode.PURPLE, 2);
                    }
                    else if (inkB.colorMode == ColorMode.YELLOW)
                    {
                        Debug.Log("C-GREEN");
                        return new Ink(Green[2], ColorMode.GREEN, 2);
                    }
                    break;
                case ColorMode.YELLOW:
                    if (inkB.colorMode == ColorMode.CYAN)
                    {
                        Debug.Log("Y-Green");
                        return new Ink(Green[3], ColorMode.GREEN, 2);
                    }
                    else if (inkB.colorMode == ColorMode.MAGENTA)
                    {
                        Debug.Log("Y-ORANGE");
                        return new Ink(Orange[3], ColorMode.ORANGE, 2);
                    }
                    break;
            }
        }
        else if (inkA.currentLevel < inkB.currentLevel)
        {
            switch (inkB.colorMode)
            {
                case ColorMode.MAGENTA:
                    if (inkA.colorMode == ColorMode.CYAN)
                    {
                        Debug.Log("M-PURPLE");
                        return new Ink(Purple[2], ColorMode.PURPLE, 2);
                    }
                    else if (inkA.colorMode == ColorMode.YELLOW)
                    {
                        Debug.Log("M-ORANGE");
                        return new Ink(Orange[2], ColorMode.ORANGE, 2);
                    }
                    break;
                case ColorMode.CYAN:
                    if (inkA.colorMode == ColorMode.MAGENTA)
                    {
                        Debug.Log("C-PURPLE");
                        return new Ink(Purple[3], ColorMode.PURPLE, 2);
                    }
                    else if (inkA.colorMode == ColorMode.YELLOW)
                    {
                        Debug.Log("C-GREEN");
                        return new Ink (Green[2], ColorMode.GREEN, 2);
                    }
                    break;
                case ColorMode.YELLOW:
                    if (inkA.colorMode == ColorMode.CYAN)
                    {
                        Debug.Log("Y-Green");
                        return new Ink(Green[3], ColorMode.GREEN, 2);
                    }
                    else if (inkB.colorMode == ColorMode.MAGENTA)
                    {
                        Debug.Log("Y-ORANGE");
                        return new Ink(Orange[3], ColorMode.ORANGE, 2);
                    }
                    break;
            }
        }
        else
        {
            if(inkA.currentLevel == Ink.Level.FULL)
            {
               if(inkA.colorMode == ColorMode.MAGENTA && inkB.colorMode == ColorMode.CYAN ||
                    inkA.colorMode == ColorMode.CYAN && inkB.colorMode == ColorMode.MAGENTA)
               {
                    return new Ink(Purple[0], ColorMode.PURPLE, 3);
               }
               else if(inkA.colorMode == ColorMode.MAGENTA && inkB.colorMode == ColorMode.YELLOW ||
                    inkA.colorMode == ColorMode.YELLOW && inkB.colorMode == ColorMode.MAGENTA)
                {
                    return new Ink(Orange[0], ColorMode.ORANGE, 3);
                }
               else if(inkA.colorMode == ColorMode.YELLOW && inkB.colorMode == ColorMode.CYAN ||
                    inkA.colorMode == ColorMode.CYAN && inkB.colorMode == ColorMode.YELLOW)
                {
                    return new Ink(Green[0], ColorMode.GREEN, 3);
                }
            }
            else
            {
                if (inkA.colorMode == ColorMode.MAGENTA && inkB.colorMode == ColorMode.CYAN ||
                    inkA.colorMode == ColorMode.CYAN && inkB.colorMode == ColorMode.MAGENTA)
                {
                    return new Ink(Purple[1], ColorMode.PURPLE, 1);
                }
                else if (inkA.colorMode == ColorMode.MAGENTA && inkB.colorMode == ColorMode.YELLOW ||
                     inkA.colorMode == ColorMode.YELLOW && inkB.colorMode == ColorMode.MAGENTA)
                {
                    return new Ink(Orange[1], ColorMode.ORANGE, 1);
                }
                else if (inkA.colorMode == ColorMode.YELLOW && inkB.colorMode == ColorMode.CYAN ||
                     inkA.colorMode == ColorMode.CYAN && inkB.colorMode == ColorMode.YELLOW)
                {
                    return new Ink(Green[1], ColorMode.GREEN, 1);
                }
            }
        }

        return new Ink();
    }

}
