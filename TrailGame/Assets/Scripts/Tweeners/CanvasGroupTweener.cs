using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CanvasGroupTweener : MonoTweener
{
    [SerializeField] protected CanvasGroup target;
    [SerializeField] protected float targetAlpha;
    [SerializeField] protected bool targetBlockRaycast;
    [SerializeField] protected bool targetInteractable;

    float originalAlpha;

    protected override Tweener LocalPlay()
    {
        if (target == null)
        {
            target = GetComponent<CanvasGroup>();
        }
        originalAlpha = target.alpha;

        target.blocksRaycasts = targetBlockRaycast;
        target.interactable = targetInteractable;
        return target.DOFade(targetAlpha, duration);
    }

    public void SetToOriginalAlpha()
    {
        target.alpha = originalAlpha;
    }

    public void SetToTargetAlpha()
    {
        target.alpha = targetAlpha;
    }
}
