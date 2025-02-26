using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameData
{
    public class InvertTile : Tile
    {
        public override void ShowTile(bool show)
        {
            if (!IsPump())
            {
                base.ShowTile(show);
            }
            else
            {
                sr.color = show ? tileInk.color : Color.clear;
            }
            if (show)
            {
                invertIcon.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
            }
            else
            {
                invertIcon.color = Color.clear;
            }
        }
        
        public void Init(MapCoord mapCoord, Tile tile, Ink ink, AnimationParams animationParams)
        {
            Coord = mapCoord;
            canTraverse = true;
            sr = tile.Sprite;
            tileInk = ink;
            invertIcon = tile.InvertIcon;
            invertIcon.DOColor(Color.black, fadeDuration).SetEase(Ease.OutExpo);

            PlayEntryAnimation(animationParams);
        }

        public override void PlayEntryAnimation(AnimationParams animationParams)
        {
            Color tileColor = IsPump() ? Color.white : tileInk.color;
            
            sr.DOColor(tileColor, animationParams.duration)
                .SetEase(animationParams.easingFunction)
                .OnStart(()=>
                {
                    animationParams.OnBegin();
                }).OnComplete(() =>
                {
                    animationParams.OnComplete();
                    if (!IsPump())
                    {
                        SetColor(tileInk, isInit: true);
                    }
                });
            Color iconColor = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
            invertIcon.DOColor(iconColor, animationParams.duration).SetEase(animationParams.easingFunction);
        }


        public override void SetColor(Ink ink, bool isInit = false)
        {
            if (ink.colorMode == ColorMode.NONE) return;
            Color iconColor = ink.colorMode == ColorMode.NONE ? Color.black : Color.white;

            invertIcon.DOColor(iconColor, fadeDuration)
                .SetEase(Ease.OutExpo)
                .OnComplete(() =>
                {
                    base.SetColor(ink, isInit);
                });
        }

        protected override void TriggerEnterEffect(Entity entity)
        {
            InvertColor(entity);
            SetColor(new Ink(entity.CurrentColorMode));
        }

        protected override void TriggerExitEffect(Entity entity)
        {
        }

        void InvertColor(Entity entity)
        {
            ColorMode inverse = FindInverse(entity.CurrentColorMode);

            Ink inverseInk = new Ink(inverse);

            entity.Ink = inverseInk;
            entity.CurrentColorMode = inverse;
            Debug.Log("SETTING INDICATORS");
            int remainingSwipes = Math.Abs(((Player)entity).ColorIndicators.Count - entity.SwipeCount);
            entity.SetIndicators(Services.ColorManager.ColorScheme.GetColor(inverse)[0], remainingSwipes);
        }

        ColorMode FindInverse(ColorMode mode)
        {
            switch(mode)
            {
                case ColorMode.MAGENTA:
                    return ColorMode.GREEN;
                case ColorMode.YELLOW:
                    return ColorMode.PURPLE;
                case ColorMode.CYAN:
                    return ColorMode.ORANGE;
                case ColorMode.GREEN:
                    return ColorMode.MAGENTA;
                case ColorMode.PURPLE:
                    return ColorMode.YELLOW;
                case ColorMode.ORANGE:
                    return ColorMode.CYAN;
                case ColorMode.BLACK:
                default:
                    return ColorMode.NONE;
            }
        }
    }
}