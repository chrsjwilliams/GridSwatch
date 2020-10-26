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

    public Ink()
    {
        color = Color.white;
        colorMode = ColorMode.NONE;
        Intensity = 0;
    }

    public Ink(Color c, ColorMode mode, int i)
    {
        color = c;
        colorMode = mode;
        Intensity = i;
    }
}
