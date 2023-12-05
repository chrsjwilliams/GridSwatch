using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace GameData
{
    public class PivotTile : Tile
    {
        [SerializeField] Direction _pivotDirection;
        public Direction PivotDirection { get { return _pivotDirection; } }

        public void Init(Tile tile, Ink ink, bool _canTraverse, Direction pivotDirection)
        {
            canTraverse = _canTraverse;
            _pivotDirection = pivotDirection;
            pivotUp = tile.PivotUp;
            pivotDown = tile.PivotDown;
            pivotLeft = tile.PivotLeft;
            pivotRight = tile.PivotRight;
            sr = tile.Sprite;

            if (!IsPump())
            {
                SetColor(ink, isInit: true);
            }
            switch (PivotDirection)
            {
                case Direction.UP:
                    pivotUp.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotDown.color = Color.clear; 
                    pivotLeft.color = Color.clear;  
                    pivotRight.color = Color.clear;

                    break;
                case Direction.DOWN:
                    pivotUp.color = Color.clear;
                    pivotDown.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotLeft.color = Color.clear;
                    pivotRight.color = Color.clear;
                    break;
                case Direction.LEFT:
                    pivotUp.color = Color.clear;
                    pivotDown.color =  Color.clear;
                    pivotLeft.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotRight.color = Color.clear;
                    break;
                case Direction.RIGHT:
                    pivotUp.color = Color.clear;
                    pivotDown.color = Color.clear;
                    pivotLeft.color = Color.clear;
                    pivotRight.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    break;
                default:
                    pivotUp.color = Color.clear;
                    pivotDown.color = Color.clear;
                    pivotLeft.color = Color.clear;
                    pivotRight.color = Color.clear;
                    break;
            }
        }

        public override void SetColor(Ink ink, bool isInit = false)
        {
            if (IsPump()) return;

            switch (PivotDirection)
            {
                case Direction.UP:
                    pivotUp.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotDown.color = Color.clear;
                    pivotLeft.color = Color.clear;
                    pivotRight.color = Color.clear;

                    break;
                case Direction.DOWN:
                    pivotUp.color = Color.clear;
                    pivotDown.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotLeft.color = Color.clear;
                    pivotRight.color = Color.clear;
                    break;
                case Direction.LEFT:
                    pivotUp.color = Color.clear;
                    pivotDown.color = Color.clear;
                    pivotLeft.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotRight.color = Color.clear;
                    break;
                case Direction.RIGHT:
                    pivotUp.color = Color.clear;
                    pivotDown.color = Color.clear;
                    pivotLeft.color = Color.clear;
                    pivotRight.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    break;
                default:
                    pivotUp.color = Color.clear;
                    pivotDown.color = Color.clear;
                    pivotLeft.color = Color.clear;
                    pivotRight.color = Color.clear;
                    break;
            }

            base.SetColor(ink, isInit);
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

        protected override void TriggerExitEffect(Entity entity)
        {
            base.TriggerExitEffect(entity);

        }
    }
}