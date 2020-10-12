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
}
