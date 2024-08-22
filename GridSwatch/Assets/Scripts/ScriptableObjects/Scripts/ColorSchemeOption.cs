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
    [ListDrawerSettings(DraggableItems = false, Expanded = true, ShowIndexLabels = false, ShowPaging = false, ShowItemCount = false, HideRemoveButton = true)]
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

    public Sprite GetColorblindPattern(ColorMode colorName)
    {
        foreach (ColorEntry entry in _colorSchemes)
        {
            if (entry.colorName == colorName)
                return entry.colorBlindPattern;
        }

        return null;
    }
}

[System.Serializable]
public struct ColorEntry
{
    public ColorMode colorName;
    public Sprite colorBlindPattern;
    [ListDrawerSettings(DraggableItems = false, Expanded = true, ShowIndexLabels = false, ShowPaging = false, ShowItemCount = false, HideRemoveButton = true)]
    public List<Color> colors;
}