using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameData
{
    public enum FillType
    {
        NONE = 0, HORIZONTAL, HORIZONTAL_END,
        VERTICLE, VERTICLE_END, ADJACENT, CARDNIAL,
        CARDINAL_END, INTERCARDINAL
    }

    public class FillTile : Tile
    {
        FillType fillType;
        List<SpriteRenderer> fillIcons = new List<SpriteRenderer>();
        ColorMode fillColor;


        public override void ShowTile(bool show)
        {
            if (!IsPump())
            {
                base.ShowTile(show);
            }
            else
            {
                sr.color = show ? Color.white : Color.clear;
            }
            if (show)
            {
                Color iconColor = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                foreach(SpriteRenderer icon in fillIcons)
                {

                    icon.color = iconColor;
                }            
            }
            else
            {
                foreach(SpriteRenderer icon in fillIcons)
                {

                    icon.color = Color.clear;
                }              
            }
        }

        public void Init(MapCoord mapCoord, Tile tile, Ink ink, FillType fill, ColorMode colorMode,
            AnimationParams animationParams)
        {
            canTraverse = true;
            Coord = mapCoord;
            fillType = fill;
            fillColor = colorMode;
            sr = tile.Sprite;
            splashFillColor = tile.SplashFillColor;

            splashFillColor.sprite = colorMode != ColorMode.NONE
                ? tile.SplashFillColorModeSprite
                : tile.SplashFillNoColorModeSprite;
            if (colorMode == ColorMode.NONE)
            {
                fillIcons.Add(splashFillColor);
            }
            else
            {
                splashFillColor.DOColor(Services.ColorManager.GetColor(colorMode), 0.25f);
            }

            switch (fill)
            {
                case FillType.HORIZONTAL:
                    horizonalIcon = tile.HorizonalIcon;

                    fillIcons.Add(horizonalIcon);
                    break;
                case FillType.HORIZONTAL_END:
                    horizonalEndIcon = tile.HorizonalEndIcon;

                    fillIcons.Add(horizonalEndIcon);
                    break;
                case FillType.VERTICLE:
                    verticleIcon = tile.VerticleIcon;

                    fillIcons.Add(verticleIcon);
                    break;
                case FillType.VERTICLE_END:
                    verticleEndIcon = tile.VerticleEndIcon;

                    fillIcons.Add(verticleEndIcon);
                    break;
                case FillType.ADJACENT:
                    adjacentIcon = tile.AdjacentIcon;

                    fillIcons.Add(adjacentIcon);
                    break;
                case FillType.CARDNIAL:
                    horizonalIcon = tile.HorizonalIcon;
                    verticleIcon = tile.VerticleIcon;

                    fillIcons.Add(horizonalIcon);
                    fillIcons.Add(verticleIcon);
                    break;
                case FillType.CARDINAL_END:
                    horizonalEndIcon = tile.HorizonalEndIcon;
                    verticleEndIcon = tile.VerticleEndIcon;

                    fillIcons.Add(horizonalEndIcon);
                    fillIcons.Add(verticleEndIcon);
                    break;
                case FillType.INTERCARDINAL:
                    interCardinal = tile.InterCardinal;

                    fillIcons.Add(interCardinal);
                    break;
                default:
                    Debug.Log("No Fill Type");
                    break;
            }

            ShowTile(false);
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
            foreach(SpriteRenderer icon in fillIcons)
            {
                
                icon.DOColor(iconColor, animationParams.duration).SetEase(animationParams.easingFunction);
            }
        }

        public override void SetColor(Ink ink, bool isInit = false)
        {
            base.SetColor(ink, isInit);
            Color iconColor = fillColor == ColorMode.NONE ? Color.black : Services.ColorManager.GetColor(fillColor);
            if(ink.colorMode != ColorMode.NONE && fillColor != ColorMode.NONE)
            {
                iconColor = Color.white;
            }
            for (int i = 0; i < fillIcons.Count; i++)
            {
                fillIcons[i].DOColor(iconColor, 0.25f)
               .SetEase(Ease.InExpo);
            }
        }

        void Fill(Entity entity)
        {
            List<Tile> tilesToFill = new List<Tile>();
            switch (fillType)
            {
                case FillType.HORIZONTAL:
                    tilesToFill.AddRange(Services.Board.GetAdjacentHorizontalTiles(this));
                    break;
                case FillType.HORIZONTAL_END:
                    tilesToFill.AddRange(Services.Board.GetRowOf(this));
                    break;
                case FillType.VERTICLE:
                    tilesToFill.AddRange(Services.Board.GetAdjacentVerticleTiles(this));
                    break;
                case FillType.VERTICLE_END:
                    tilesToFill.AddRange(Services.Board.GetColumnOf(this));
                    break;
                case FillType.ADJACENT:
                    tilesToFill.AddRange(Services.Board.GetAllAdjacentTiles(this));
                    break;
                case FillType.CARDNIAL:
                    tilesToFill.AddRange(Services.Board.GetAdjacentHorizontalTiles(this));
                    tilesToFill.AddRange(Services.Board.GetAdjacentVerticleTiles(this));
                    break;
                case FillType.CARDINAL_END:
                    tilesToFill.AddRange(Services.Board.GetColumnOf(this));
                    tilesToFill.AddRange(Services.Board.GetRowOf(this));
                    break;
                case FillType.INTERCARDINAL:
                    tilesToFill.AddRange(Services.Board.GetInterCardinalOf(this));
                    break;
                default:
                    Debug.Log("No Fill Type");
                    break;
            }

            ColorMode effectFill = fillColor == ColorMode.NONE ? entity.CurrentColorMode : fillColor;
            Ink newInk = new Ink(effectFill);
            foreach (Tile tile in tilesToFill)
            {
                tile.SetColor(newInk);
            }
        }

        protected override void TriggerEnterEffect(Entity entity)
        {
            if (!canTraverse) return;
            if (entity.CurrentColorMode != ColorMode.NONE &&
            (CurrentColorMode == ColorMode.NONE ||
            CurrentColorMode == ColorMode.MAGENTA ||
            CurrentColorMode == ColorMode.YELLOW ||
            CurrentColorMode == ColorMode.CYAN))
            {
                ColorMode effectFill = fillColor == ColorMode.NONE ? entity.CurrentColorMode : fillColor;
                Ink newInk = new Ink(effectFill);
                SetColor(newInk);
            }
            Fill(entity);
        }

        protected override void TriggerExitEffect(Entity entity)
        {
            if (!canTraverse) return;
            if (entity.CurrentColorMode != ColorMode.NONE &&
            (CurrentColorMode == ColorMode.GREEN ||
            CurrentColorMode == ColorMode.ORANGE ||
            CurrentColorMode == ColorMode.PURPLE))
            {
                ColorMode effectFill = fillColor == ColorMode.NONE ? entity.CurrentColorMode : fillColor;
                Ink newInk = new Ink(effectFill);
                SetColor(newInk);
            }
        }
    }
}