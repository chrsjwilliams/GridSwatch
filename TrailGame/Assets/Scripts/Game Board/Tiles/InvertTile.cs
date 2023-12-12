using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameData
{
    public class InvertTile : Tile
    {
        public void Init(MapCoord mapCoord, Tile tile, Ink ink)
        {
            Coord = mapCoord;
            canTraverse = true;
            sr = tile.Sprite;
            tileInk = ink;
            invertIcon = tile.InvertIcon;
            invertIcon.DOColor(Color.black, 0.25f).SetEase(Ease.InExpo);

            if (!IsPump())
            {
                SetColor(ink, isInit: true);
            }
        }

        public override void SetColor(Ink ink, bool isInit = false)
        {
            invertIcon.DOColor(Color.white, 0.25f)
                .SetEase(Ease.InExpo)
                .OnComplete(() =>
                {
                    base.SetColor(ink, isInit);
                });
        }

        protected override void TriggerEnterEffect(Entity entity)
        {
            InvertColor(entity);
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
            entity.SetIndicators(Services.ColorManager.ColorScheme.GetColor(inverse)[0]);
            entity.PrevColorMode = inverse;
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