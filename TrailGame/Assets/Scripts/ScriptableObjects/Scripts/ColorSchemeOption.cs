using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "Color Scheme"
                , menuName = "Data/Color Scheme Option")]
public class ColorSchemeOption : ScriptableObject
{
    
    [SerializeField] Color _errorColor;
    public Color ErrorColor { get { return _errorColor; } }
    [SerializeField] List<ColorEntry> _colorSchemes;

    public List<Color> GetColor(ColorMode colorName)
    {
        foreach(ColorEntry entry in _colorSchemes)
        {
            if (entry.colorName == colorName)
                return entry.colors;
        }

        return null;
    }
}

[System.Serializable]
public struct ColorEntry
{
    public ColorMode colorName;
    public List<Color> colors;
}