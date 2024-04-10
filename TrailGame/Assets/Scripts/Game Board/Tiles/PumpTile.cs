using UnityEngine;
using DG.Tweening;

namespace GameData
{
    public class PumpTile : Tile
    {
        public ColorMode PumpColor;


        public override void ShowTile(bool show)
        {
            if (show)
            {
                pumpIndicator.color = tileInk.color;
            }
            else
            {
                pumpIndicator.color = Color.clear;
            }
            sr.color = Color.clear;

        }

        public void Init(MapCoord mapCoord, Tile tile, Ink initInk, bool _canTraverse, AnimationParams animationParams)
        {
            Coord = mapCoord;
            canTraverse = _canTraverse;
            PumpColor = initInk.colorMode;
            sr = tile.Sprite;
            tileInk = initInk;
            pumpIndicator = tile.PumpIndicator;
            ShowTile(false);
            PlayEntryAnimation(animationParams);
        }

        public override void PlayEntryAnimation(AnimationParams animationParams)
        {
            sr.DOColor(Color.clear, animationParams.duration)
                .SetEase(animationParams.easingFunction)
                .OnStart(()=>
                {
                    animationParams.OnBegin();
                }).OnComplete(() =>
                {
                    animationParams.OnComplete();
                });            
            pumpIndicator.DOColor(tileInk.color, animationParams.duration).SetEase(animationParams.easingFunction);
        }

        public override void SetColor(Ink ink, bool isInit = false)
        {
        }

        protected override void TriggerEnterEffect(Entity entity)
        {
            entity.Ink = tileInk;
            entity.CurrentColorMode = PumpColor;
            entity.ResetIntensitySwipes();
            entity.SetIndicators(Services.ColorManager.ColorScheme.GetColor(PumpColor)[0]);
        }

        protected override void TriggerExitEffect(Entity entity)
        {

        }
    }
}