using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    public class InitalizeAds : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField] private string _androidGameId;
        [SerializeField] private string _iOSGameId;
        [SerializeField] private bool _testMode = true;
        private string _gameId;

        public void InitializeAds(Action callback)
        {
        #if UNITY_IOS
            _gameId = _iOSGameId;
        #elif UNITY_ANDROID
            _gameId = _androidGameId;
        #elif UNITY_EDITOR
            _gameId = _iOSGameId;
        #endif

            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, _testMode, this);
            }
        }
        
        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }
 
        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}
