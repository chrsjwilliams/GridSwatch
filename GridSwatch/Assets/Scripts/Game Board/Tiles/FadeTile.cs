using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameData
{
    public class FadeTile : Tile
    {
        private Direction direction;

        private int _fadeCount;
        private bool _diffused = false;

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
                Color textColor = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                _fadeCounter.color = textColor;
            }
            else
            {
                _fadeCounter.color = Color.clear;
            }
        }

        public void Init(MapCoord mapCoord, Tile tile, Ink ink, int fadeCount, AnimationParams animationParams)
        {
            Coord = mapCoord;
            canTraverse = true;
            sr = tile.Sprite;
            tileInk = ink;
            _fadeCounter = tile.FadeCounter;
            _fadeCount = fadeCount;
            _fadeCounter.text = _fadeCount + "";
            direction = Direction.NONE;
            _diffused = false;
            Services.EventManager.Register<SwipeEvent>(OnSwipe);
            ShowTile(false);
            PlayEntryAnimation(animationParams);
        }

        public void PlayEntryAnimation(AnimationParams animationParams)
        {
            Color tileColor = IsPump() ? Color.white : tileInk.color;
            sr.DOColor(tileColor, animationParams.duration)
                .SetEase(animationParams.easingFunction)
                .OnStart(() => { animationParams.OnBegin(); }).OnComplete(() =>
                {
                    animationParams.OnComplete();
                    if (!IsPump())
                    {
                        SetColor(tileInk, isInit: true);
                    }
                });
            Color textColor = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
            _fadeCounter.DOColor(textColor, animationParams.duration).SetEase(animationParams.easingFunction);

        }

        private void OnDestroy()
        {
            Services.EventManager.Unregister<SwipeEvent>(OnSwipe);
        }

        private void OnSwipe(SwipeEvent e)
        {
            if (AxisSwipeChange(e) && !_diffused)
            {
                _fadeCount -= 1;
                if (_fadeCount < 0)
                {
                    Ink black = new Ink(ColorMode.BLACK);
                    SetColor(black);
                    _fadeCount = 0;
                    Services.EventManager.Unregister<SwipeEvent>(OnSwipe);
                }
                _fadeCounter.text = _fadeCount + "";
            }
        }
        
        public bool AxisSwipeChange(SwipeEvent e)
        {
            return ((direction == Direction.LEFT || direction == Direction.RIGHT) &&
                    (e.gesture.CurrentDirection == Direction.UP || e.gesture.CurrentDirection == Direction.DOWN)) ||
                   ((direction == Direction.UP || direction == Direction.DOWN) &&
                    (e.gesture.CurrentDirection == Direction.LEFT || e.gesture.CurrentDirection == Direction.RIGHT)) ||
                   (direction == Direction.NONE && e.gesture.CurrentDirection != Direction.NONE);
        }

        public override void SetColor(Ink ink, bool isInit = false)
        {
            if (isInit)
            {
                base.SetColor(ink, isInit);
            }
            else
            {
                Services.EventManager.Unregister<SwipeEvent>(OnSwipe);
                _diffused = true;
                FadeCounter.DOFade(0, 0.25f)
                    .SetEase(Ease.OutExpo)
                    .OnComplete(() =>
                    {
                        base.SetColor(ink, isInit);
                    });
            }
        }
    }
}