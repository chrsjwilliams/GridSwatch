using System;
using DG.Tweening;

public struct AnimationParams
{
    public float duration;
    public Ease easingFunction;
    public Action OnBegin;
    public Action OnComplete;
}
