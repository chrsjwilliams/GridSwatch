using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;

public class TextMeshProColorTweener : MonoTweener
{

    [SerializeField] TextMeshPro text;
    [SerializeField] ColorMode colorMode;
    [SerializeField, ShowIf("colorMode", ColorMode.RGB)] Color TargetColor;

    [ShowIf("colorMode", ColorMode.HSV), InfoBox("X: Change Hue by, Y: Change Saturation by, Z: Change Value by")]
    [SerializeField, ShowIf("colorMode", ColorMode.HSV)] Vector3 HSVDelta;

    [SerializeField, ShowIf("colorMode", ColorMode.HSV)] bool hdr;


    Color _originalColor;
    protected override Tweener LocalPlay()
    {
        _originalColor = text.color;
        if (colorMode == ColorMode.HSV)
        {
            float h, s, v;
            Color imgColor = text.color;
            Color.RGBToHSV(new Color(imgColor.r, imgColor.g, imgColor.b), out h, out s, out v);
            TargetColor = Color.HSVToRGB(h + HSVDelta.x, s + HSVDelta.y, v + HSVDelta.z, hdr);

        }

        return text.DOColor(TargetColor, duration).SetEase(easing);

    }

    public void SetToOriginalColor()
    {
        text.color = _originalColor;
    }

    public void SetToTargetColor()
    {
        text.color = TargetColor;
    }

    public enum ColorMode
    {
        RGB, HSV
    }
}
