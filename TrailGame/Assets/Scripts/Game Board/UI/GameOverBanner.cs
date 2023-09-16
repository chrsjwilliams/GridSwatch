using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBanner : MonoBehaviour
{
    [SerializeField] MonoTweener showBannerTweener;
    [SerializeField] MonoTweener hideBannerTweener;

    public void ShowBanner()
    {
        showBannerTweener?.Play();
    }

    public void HideBanner()
    {
        hideBannerTweener?.Play();
    }
}
