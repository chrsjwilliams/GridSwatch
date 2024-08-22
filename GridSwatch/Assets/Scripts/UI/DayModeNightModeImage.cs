using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DayModeNightModeImage : MonoBehaviour
{

    [SerializeField] Color dayModeColor;
    [SerializeField] Color nightModeColor;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (DayNightModeButton.CurrentDisplayMode == DayNightModeButton.DisplayMode.DAY)
        {
            image.color = dayModeColor;
        }
        else if (DayNightModeButton.CurrentDisplayMode == DayNightModeButton.DisplayMode.NIGHT)
        {
            image.color = nightModeColor;
        }
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        DayNightModeButton.DisplayModeChanged += OnDisplayModeChanged;
    }

    private void OnDisable()
    {
        DayNightModeButton.DisplayModeChanged -= OnDisplayModeChanged;

    }

    private void OnDisplayModeChanged(DayNightModeButton.DisplayMode mode)
    {
        if(mode == DayNightModeButton.DisplayMode.DAY)
        {
            image.DOColor(dayModeColor, 0.33f).SetEase(Ease.InOutQuint);
        }
        else if(mode == DayNightModeButton.DisplayMode.NIGHT)
        {
            image.DOColor(nightModeColor, 0.33f).SetEase(Ease.InOutQuint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
