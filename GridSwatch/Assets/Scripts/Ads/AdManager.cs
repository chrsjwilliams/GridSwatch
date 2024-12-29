using System;
using UnityEngine;

namespace Ads
{
    public enum AdType {BANNER, INTERSTITIAL, REWARDED}
    public class AdManager : MonoBehaviour
    {
        [SerializeField] private InitalizeAds intializeAds;
        [SerializeField] private  InterstitialAds interstitialAds;
        [SerializeField] private  BannerAds bannerAds;
        [SerializeField] private  RewardedAds rewardedAds;

        private bool _enableAds = false;
        private bool readyToShowAds = false;
        public static AdManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Initializing Ads...");
            intializeAds.InitializeAds(() =>
            {
                bannerAds.LoadBanner();
                interstitialAds.LoadAd();
                rewardedAds.LoadAd();
                readyToShowAds = true;
            });
        }

        public void ShowAd(AdType type, Action callback)
        {
            if (!_enableAds && !readyToShowAds)
            {
                callback?.Invoke();
                return;
            }
            
            switch (type)
            {
                case AdType.BANNER:
                    bannerAds.ShowBannerAd();
                    callback?.Invoke();
                    break;
                case AdType.INTERSTITIAL:
                    interstitialAds.ShowAd(callback);
                    break;
                case AdType.REWARDED:
                    rewardedAds.ShowAd(callback);
                    break;
            }
        }
    }
}
