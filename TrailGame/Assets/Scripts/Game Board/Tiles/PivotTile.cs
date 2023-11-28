using System.Collections;
using UnityEngine;

namespace GameData
{
    public class PivotTile : Tile
    {
        [SerializeField] Swipe.Direction _pivotDirection;
        public Swipe.Direction PivotDirection { get { return _pivotDirection; } }

        public void Init(Tile tile, Ink ink, bool _canTraverse, Swipe.Direction pivotDirection)
        {
            tileInk = ink;
            canTraverse = _canTraverse;
            _pivotDirection = pivotDirection;
            pivotUp = tile.PivotUp;
            pivotDown = tile.PivotDown;
            pivotLeft = tile.PivotLeft;
            pivotRight = tile.PivotRight;
            sr = tile.Sprite;

            Debug.Log("CAN TRAVERSE PIVOT: " + canTraverse);

            switch (PivotDirection)
            {
                case Swipe.Direction.UP:
                    pivotUp.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotDown.color = Color.clear; 
                    pivotLeft.color = Color.clear;  
                    pivotRight.color = Color.clear;

                    break;
                case Swipe.Direction.DOWN:
                    pivotUp.color = Color.clear;
                    pivotDown.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotLeft.color = Color.clear;
                    pivotRight.color = Color.clear;
                    break;
                case Swipe.Direction.LEFT:
                    pivotUp.color = Color.clear;
                    pivotDown.color =  Color.clear;
                    pivotLeft.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotRight.color = Color.clear;
                    break;
                case Swipe.Direction.RIGHT:
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
            base.SetColor(ink, isInit);

            switch (PivotDirection)
            {
                case Swipe.Direction.UP:
                    pivotUp.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotDown.color = Color.clear;
                    pivotLeft.color = Color.clear;
                    pivotRight.color = Color.clear;

                    break;
                case Swipe.Direction.DOWN:
                    pivotUp.color = Color.clear;
                    pivotDown.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotLeft.color = Color.clear;
                    pivotRight.color = Color.clear;
                    break;
                case Swipe.Direction.LEFT:
                    pivotUp.color = Color.clear;
                    pivotDown.color = Color.clear;
                    pivotLeft.color = CurrentColorMode == ColorMode.NONE ? Color.black : Color.white;
                    pivotRight.color = Color.clear;
                    break;
                case Swipe.Direction.RIGHT:
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

        IEnumerator Pivot(Entity entity)
        {
            entity.receiveInput = false;
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