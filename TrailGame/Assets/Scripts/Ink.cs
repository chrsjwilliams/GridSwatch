using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink 
{
    public enum Level { DIM = 0, FULL}

    public Level currentLevel;
    public Color color;
    public ColorMode colorMode;
    private int intensity;
    public int Intensity {
        get { return intensity; }
        set {
            if(value > 1)
            {
                currentLevel = Level.FULL;
            }
            else
            {
                currentLevel = Level.DIM;
            }
            intensity = value;
        }
    }

    public Ink(bool canTraverse = false){
        if(canTraverse) color = Color.white;
        else color = Color.clear;
        colorMode = ColorMode.NONE;
        Intensity = 0;
    }
    /*
    public Ink()
    {
        color = Color.white;
        colorMode = ColorMode.NONE;
        Intensity = 0;
    }
    */

    public Ink(Color c, ColorMode mode, int i)
    {
        color = c;
        colorMode = mode;
        Intensity = i;
        if(colorMode == ColorMode.BLACK)
        {
            if (currentLevel == Level.FULL)
                color = Services.ColorManager.Black[0];
            else
                color = Services.ColorManager.Black[1];
        }
    }

    public Ink(ColorMode mode)
    {
        colorMode = mode;
        switch(colorMode)
        {
            case ColorMode.MAGENTA:
                color = Services.ColorManager.Magenta[0];
                break;
            case ColorMode.CYAN:
                color = Services.ColorManager.Cyan[0];
                break;
            case ColorMode.YELLOW:
                color = Services.ColorManager.Yellow[0];
                break;
            case ColorMode.GREEN:
                color = Services.ColorManager.Green[0];
                break;
            case ColorMode.ORANGE:
                color = Services.ColorManager.Orange[0];
                break;
            case ColorMode.PURPLE:
                color = Services.ColorManager.Purple[0];
                break;
            case ColorMode.BLACK:
                color = Services.ColorManager.Black[0];
                break;
        }
        Intensity = Player.MAX_INTENSITY_LEVEL;
    }
}
