using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBanner : MonoBehaviour
{
    [SerializeField] MonoTweener showBannerTweener;
    [SerializeField] MonoTweener hideBannerTweener;

    [SerializeField] private CanvasGroup nextButton;
    
    public void ShowBanner(bool hasNextMap)
    {
        showBannerTweener?.Play();

        nextButton.interactable = nextButton.blocksRaycasts = hasNextMap;
        nextButton.alpha = hasNextMap ? 1 : 0;
    }

    public void HideBanner()
    {
        hideBannerTweener?.Play();
    }
}
