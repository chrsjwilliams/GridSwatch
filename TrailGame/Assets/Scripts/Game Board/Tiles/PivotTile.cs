using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace GameData
{
    public class PivotTile : Tile
    {
        [SerializeField] Direction _pivotDirection;
        public Direction PivotDirection { get { return _pivotDirection; } }
        private SpriteRenderer _pivotSprite;
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
                _pivotSprite.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
            }
            else
            {
                _pivotSprite.color = Color.clear;
            }
        }

        public void Init(MapCoord mapCoord, Tile tile, Ink ink, bool _canTraverse, Direction pivotDirection, AnimationParams animationParams)
        {
            Coord = mapCoord;
            canTraverse = _canTraverse;
            _pivotDirection = pivotDirection;
            pivotUp = tile.PivotUp;
            pivotDown = tile.PivotDown;
            pivotLeft = tile.PivotLeft;
            pivotRight = tile.PivotRight;
            sr = tile.Sprite;
            tileInk = ink;
            
            pivotUp.color = Color.clear;
            pivotDown.color = Color.clear; 
            pivotLeft.color = Color.clear;  
            pivotRight.color = Color.clear;
            
            switch (PivotDirection)
            {
                case Direction.UP:
                    _pivotSprite = pivotUp;
                    break;
                case Direction.DOWN:
                    _pivotSprite = pivotDown;
                    break;
                case Direction.LEFT:
                    _pivotSprite = pivotLeft;
                    break;
                case Direction.RIGHT:
                    _pivotSprite = pivotRight;
                    break;
                default:
                    _pivotSprite = pivotUp;
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
            _pivotSprite.DOColor(iconColor, animationParams.duration).SetEase(animationParams.easingFunction);
        }

        public override void SetColor(Ink ink, bool isInit = false)
        {
            if (IsPump()) return;
            
            base.SetColor(ink, isInit);
            _pivotSprite.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
        }

        IEnumerator Pivot(Entity entity)
        {
            entity.transform.DOLocalMove(new Vector3(transform.localPosition.x, transform.localPosition.y, entity.transform.localPosition.z), 0.1f);
            entity.receiveInput = false;
            entity.direction = Direction.NONE;
            yield return new WaitForSeconds(0.2f);
            entity.PivotDirection(PivotDirection);
            entity.receiveInput = true;
        }

        protected override void TriggerEnterEffect(Entity entity)
        {
            base.TriggerEnterEffect(entity);
            StartCoroutine(Pivot(entity));
        }
    }
}