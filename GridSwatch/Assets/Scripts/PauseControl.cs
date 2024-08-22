using System;
using UnityEngine;
using DG.Tweening;

public static class PauseControl
{
    public static event Action<bool> OnPause;

    public static bool isPaused { get { return _pauseCount > 0; } }

    static int _pauseCount = 0;
    static Tween unpauseTween;

    static public void Pause()
    {
        _pauseCount++;
        if (_pauseCount == 1)
        {
            if (unpauseTween != null && unpauseTween.IsPlaying())
            {
                unpauseTween.Kill();
            }
            Time.timeScale = 0f;
            OnPause?.Invoke(true);
        }
    }

    static public void UnPause()
    {
        _pauseCount--;
        if (_pauseCount == 0)
        {
            unpauseTween = DOTween.To(
                                        () => Time.timeScale
                                        , scale => Time.timeScale = scale
                                        , 1f
                                        , 1f
                                        )
                                    .SetUpdate(UpdateType.Normal, true);
            OnPause?.Invoke(false);
        }
    }

}
