using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameData
{
    // Only allow player to pass if the player has the 
    // specified color
    public class GateTile : Tile
    {
        public enum Comparison{ EQUAL, NOT_EQUAL }
        
        private ColorMode _gateColor;
        private bool _negationGate;
        public void Init(MapCoord mapCoord, Tile tile, Ink ink, ColorMode gateColor, bool notGate, AnimationParams animationParams)
        {
            Coord = mapCoord;
            canTraverse = true;
            sr = tile.Sprite;
            _gateColor = gateColor;
            _negationGate = notGate;
            tileInk = ink;
            gateIcon = tile.GateIcon;
            gateIcon.sprite = notGate ? tile.GateIcons[(int)Comparison.NOT_EQUAL] : tile.GateIcons[(int)Comparison.EQUAL];
            
            gateIcon.DOColor(Services.ColorManager.GetColor(gateColor), 0.25f).SetEase(Ease.InExpo);

            PlayEntryAnimation(animationParams);
        }

        public override bool GetCanTraverse(Entity entity)
        {
            if ((!_negationGate && entity.CurrentColorMode != _gateColor) ||
                (_negationGate && entity.CurrentColorMode == _gateColor))
            {
                return false;
            }
            return true;
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

            if (!_negationGate)
            {
                Color iconColor = CurrentColorMode == ColorMode.NONE ? Services.ColorManager.GetColor(_gateColor) : Color.white;
                gateIcon.DOColor(iconColor, animationParams.duration).SetEase(animationParams.easingFunction);
            }
        }
        
        public override void SetColor(Ink ink, bool isInit = false)
        {
            if (ink.colorMode != ColorMode.NONE && !_negationGate)
            {
                gateIcon.DOColor(Color.white, fadeDuration)
                    .SetEase(Ease.OutExpo)
                    .OnComplete(() => { base.SetColor(ink, isInit); });
            }
            else
            {
                base.SetColor(ink, isInit);
            }
        }
        
        protected override void TriggerEnterEffect(Entity entity)
        {
            
            base.TriggerEnterEffect(entity);
        }

        protected override void TriggerExitEffect(Entity entity)
        {
            base.TriggerExitEffect(entity);
        }
    }
}