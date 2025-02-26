﻿using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

namespace GameData
{
    public class Tile : MonoBehaviour
    {
        #region Properties

        protected float fadeDuration = 1f;
        public MapCoord Coord { get; protected set; }
        public bool canTraverse { get; protected set; }
        public int intensity { get; protected set; }
        public Ink tileInk { get; protected set; }
        public ColorMode CurrentColorMode { get; protected set; }

        [SerializeField] protected SpriteRenderer sr;
        public SpriteRenderer Sprite { get { return sr; } }
        [SerializeField] protected SpriteRenderer pumpIndicator;
        public SpriteRenderer PumpIndicator { get { return pumpIndicator; } }
        [SerializeField] protected SpriteRenderer pivotUp;
        public SpriteRenderer PivotUp { get { return pivotUp; } }
        [SerializeField] protected SpriteRenderer pivotDown;
        public SpriteRenderer PivotDown { get { return pivotDown; } }
        [SerializeField] protected SpriteRenderer pivotLeft;
        public SpriteRenderer PivotLeft { get { return pivotLeft; } }
        [SerializeField] protected SpriteRenderer pivotRight;
        public SpriteRenderer PivotRight { get { return pivotRight; } }
        [SerializeField] protected SpriteRenderer wrapArrow;
        public SpriteRenderer WrapArrow { get { return wrapArrow; } }
        [SerializeField] protected SpriteRenderer invertIcon;
        public SpriteRenderer InvertIcon { get { return invertIcon; } }
        [SerializeField] public Sprite SplashFillColorModeSprite;
        [SerializeField] public Sprite SplashFillNoColorModeSprite;
        [SerializeField] protected SpriteRenderer splashFillColor;
        public SpriteRenderer SplashFillColor { get { return splashFillColor; } }
        [SerializeField] protected SpriteRenderer horizonalIcon;
        public SpriteRenderer HorizonalIcon { get { return horizonalIcon; } }
        [SerializeField] protected SpriteRenderer horizonalEndIcon;
        public SpriteRenderer HorizonalEndIcon { get { return horizonalEndIcon; } }
        [SerializeField] protected SpriteRenderer verticleEndIcon;
        public SpriteRenderer VerticleIcon { get { return verticleIcon; } }
        [SerializeField] protected SpriteRenderer verticleIcon;
        public SpriteRenderer VerticleEndIcon { get { return verticleEndIcon; } }
        [SerializeField] protected SpriteRenderer interCardinal;
        public SpriteRenderer InterCardinal { get { return interCardinal; } }
        [SerializeField] protected SpriteRenderer interCardinalEnd;
        public SpriteRenderer InterCardinalEnd { get { return interCardinalEnd; } }
        [SerializeField] protected SpriteRenderer adjacentIcon;
        public SpriteRenderer AdjacentIcon { get { return adjacentIcon; } }
        [SerializeField] protected TextMeshProUGUI _fadeCounter;
        public TextMeshProUGUI FadeCounter { get { return _fadeCounter; } }
        [SerializeField] protected SpriteRenderer gateIcon;
        public SpriteRenderer GateIcon { get { return gateIcon; } }
        public Sprite[] GateIcons;
        #endregion

        protected Quaternion GetRandomRotation()
        {
            float rand = Random.Range(0, 1000);
            float mod = rand % 4;
            float newRotation = 90 * mod;

            return Quaternion.Euler(0, 0, newRotation);
        }
        
        
        public virtual void Init(MapCoord mapCoord, Ink initInk, bool _canTraverse, AnimationParams animationParams)
        {
            Coord = mapCoord;
            transform.localRotation = GetRandomRotation();
            sr.transform.localRotation = GetRandomRotation();
            
            canTraverse = _canTraverse;
            tileInk = initInk;
            ShowTile(false);
            PlayEntryAnimation(animationParams);
        }

        public virtual bool GetCanTraverse(Entity entity)
        {
            return canTraverse;
        }

        public virtual void ShowTile(bool show)
        {
            if (show)
            {
                sr.color = tileInk.color;
            }
            else
            {
                sr.color = Color.clear;
            }
        }
        
        public virtual void PlayEntryAnimation(AnimationParams animationParams)
        {
            sr.DOColor(tileInk.color, animationParams.duration)
                .SetEase(animationParams.easingFunction)
                .OnStart(()=>
                {
                    animationParams.OnBegin();
                }).OnComplete(() =>
                {
                    animationParams.OnComplete();
                    SetColor(tileInk, true);
                });
                
        }

        public virtual void PlayExitAnimation(AnimationParams animationParams)
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
        }
        
        public void SetTraversal(bool b) { canTraverse = b; }

        public bool IsPump() { return null != GetComponent<PumpTile>(); }

        public virtual void SetColor(Ink ink, bool isInit = false)
        {
            if (isInit)
                tileInk = ink;

            if (!canTraverse)
            {
                CurrentColorMode = ColorMode.BLACK;
                sr.DOColor(tileInk.color, fadeDuration).SetEase(Ease.OutExpo);
                return;
            }

            if (ShouldMixColors(ink))
            {
                Ink oldTileInk = tileInk;
                tileInk = Services.ColorManager.MixColors(tileInk, ink);
                if(oldTileInk.colorMode != tileInk.colorMode)
                {
                    Services.Board.CurrentFillAmount[(int)oldTileInk.colorMode]--;
                }

                sr.color = tileInk.color;
                sr.DOColor(tileInk.color, fadeDuration).SetEase(Ease.OutExpo);
                
                if (tileInk.colorMode != CurrentColorMode && !(this is PumpTile))
                    Services.Board.CurrentFillAmount[(int)tileInk.colorMode]++;

                CurrentColorMode = tileInk.colorMode;

            }
            else if (CanOverwriteColor(ink))
            {
                if (!(this is PumpTile))
                {
                    if (ink.colorMode != tileInk.colorMode)
                        Services.Board.CurrentFillAmount[(int)tileInk.colorMode]--;
                }
                tileInk = ink;

                sr.DOColor(tileInk.color, fadeDuration).SetEase(Ease.OutExpo);
               
                if (tileInk.colorMode != CurrentColorMode && !(this is PumpTile))
                    Services.Board.CurrentFillAmount[(int)tileInk.colorMode]++;

                CurrentColorMode = tileInk.colorMode;

            }
            else
            {
                sr.DOColor(tileInk.color, fadeDuration).SetEase(Ease.OutExpo);
                if (tileInk.colorMode != CurrentColorMode && !(this is PumpTile))
                    Services.Board.CurrentFillAmount[(int)tileInk.colorMode]++;

                CurrentColorMode = tileInk.colorMode;
            }

            if (CurrentColorMode == ColorMode.BLACK)
            {
                canTraverse = false;
            }
        }

        public bool CanOverwriteColor(Ink newInk)
        {
            return  //  Tile has no color
                    (tileInk == null) ||
                    (tileInk.colorMode == ColorMode.NONE) ||
                    //  Tile is not Black Color
                    ((tileInk.colorMode != ColorMode.BLACK) &&
                    //  New color has higher intensity
                    (newInk.colorMode == tileInk.colorMode && newInk.Intensity > tileInk.Intensity)) ||
                    // Primary colors cannot overwrite secondary colors
                    ((  tileInk.colorMode == ColorMode.GREEN ||
                        tileInk.colorMode == ColorMode.PURPLE ||
                        tileInk.colorMode == ColorMode.ORANGE) &&
                        (newInk.colorMode == ColorMode.CYAN ||
                        newInk.colorMode == ColorMode.MAGENTA ||
                        newInk.colorMode == ColorMode.YELLOW));
            //  Should lower intensity colors mix to make higher intensity colors?
        }

        public bool ShouldMixColors(Ink newInk)
        {
            return tileInk.colorMode != ColorMode.NONE &&
                    newInk.colorMode != ColorMode.NONE &&
                    tileInk.colorMode != newInk.colorMode;
        }


        protected virtual void TriggerEnterEffect(Entity entity)
        {
            if (!canTraverse) return;
            if (entity.CurrentColorMode != ColorMode.NONE &&
            (CurrentColorMode == ColorMode.NONE ||
            CurrentColorMode == ColorMode.MAGENTA ||
            CurrentColorMode == ColorMode.YELLOW ||
            CurrentColorMode == ColorMode.CYAN))
            {
                Ink newInk = new Ink(entity.CurrentColorMode);
                SetColor(newInk);
            }
        }

        protected virtual void TriggerExitEffect(Entity entity)
        {
            // WARNING: THIS MIGHT BE BROKEN
            if (!canTraverse) return;
            if (entity.CurrentColorMode != ColorMode.NONE &&
            (CurrentColorMode == ColorMode.GREEN ||
            CurrentColorMode == ColorMode.ORANGE ||
            CurrentColorMode == ColorMode.PURPLE))
            {
                Ink newInk = new Ink(entity.CurrentColorMode);
                SetColor(newInk);
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            Player player = collision.GetComponent<Player>();
            if (player == null) return;

            TriggerEnterEffect(player);
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            Player player = collision.GetComponent<Player>();
            if (player == null) return;

            TriggerExitEffect(player);
        }

    }
}