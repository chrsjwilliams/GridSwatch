using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using System;
using static UnityEngine.EventSystems.EventTrigger;

namespace GameData
{
    public class WrapTile : Tile
    {
        Direction _wrapDirection;
        public Direction WrapDirection { get { return _wrapDirection; } }
        Entity containedEntity;

        private void OnEnable()
        {
            Services.EventManager.Register<SwipeEvent>(OnSwipe);
        }

        private void OnDisable()
        {
            Services.EventManager.Unregister<SwipeEvent>(OnSwipe);
        }

        private void OnSwipe(SwipeEvent e)
        {
            if (e.gesture.CurrentDirection != WrapDirection || containedEntity == null) return;
            containedEntity.direction = e.gesture.CurrentDirection;
            StartCoroutine(Wrap(containedEntity));

        }

        public void Init(Tile tile, Ink ink, Direction direction)
        {

            canTraverse = true;
            _wrapDirection = direction;
            sr = tile.Sprite;
            wrapArrow = tile.WrapArrow;
            tileInk = ink;
            // right = 0
            // up = 90
            // left = 180
            // down 270
            if (!IsPump())
            {
                SetColor(ink, isInit: true);
            }
            switch (direction)
            {
                case Direction.RIGHT:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Direction.UP:
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case Direction.LEFT:
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case Direction.DOWN:
                    transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                default:
                    break;
            }
        }

        public override void SetColor(Ink ink, bool isInit = false)
        {
            if (IsPump()) return;

            base.SetColor(ink, isInit);
            wrapArrow.DOColor(tileInk.color, 0.0f).SetDelay(0.05f).SetEase(Ease.InExpo);
        }

        // ~TODO: CLEAN THIS CHAIN OF FUNCTIONS UP PLEASE
        IEnumerator Wrap(Entity entity)
        {
            entity.receiveInput = false;
            entity.canMove = false;

            Vector2 _wrapPos = FindWrapPosition();
            yield return new WaitForSeconds(0.2f);

            TweenCallback setPos = ()=>
            {
                switch (WrapDirection)
                {
                    case Direction.LEFT:
                        entity.transform.DOLocalMoveX(_wrapPos.x, 0.5f)
                        .OnStart(()=> {
                            entity.Show(true);
                        })
                        .SetDelay(0.2f)
                        .OnComplete(() => {
                            entity.coord = new MapCoord(_wrapPos.x, _wrapPos.y);
                            entity.receiveInput = true;
                            entity.canMove = true;
                            if (entity is Player)
                            {
                                ((Player)entity).Move(WrapDirection);
                            }
                        });
                        break;
                    case Direction.RIGHT:
                        entity.transform.DOLocalMoveX(_wrapPos.x, 0.5f)
                        .OnStart(() => {
                            entity.Show(true);
                        })
                        .SetDelay(0.2f)
                        .OnComplete(() => {
                            entity.coord = new MapCoord(_wrapPos.x, _wrapPos.y);
                            entity.receiveInput = true;
                            entity.canMove = true;
                            if (entity is Player)
                            {
                                ((Player)entity).Move(WrapDirection);
                            }
                        });
                        break;
                    case Direction.DOWN:
                        entity.transform.DOLocalMoveY(_wrapPos.y, 0.5f)
                        .OnStart(() => {
                            entity.Show(true);
                        })
                        .SetDelay(0.2f)
                        .OnComplete(() => {
                            entity.coord = new MapCoord(_wrapPos.x, _wrapPos.y);
                            entity.receiveInput = true;
                            entity.canMove = true;
                            if (entity is Player)
                            {
                                ((Player)entity).Move(WrapDirection);
                            }
                        });
                        break;
                    case Direction.UP:
                        entity.transform.DOLocalMoveY(_wrapPos.y, 0.5f)
                        .OnStart(() => {
                            entity.Show(true);
                        })
                        .SetDelay(0.2f)
                        .OnComplete(() => {
                            entity.coord = new MapCoord(_wrapPos.x, _wrapPos.y);
                            entity.receiveInput = true;
                            entity.canMove = true;
                            if (entity is Player)
                            {
                                ((Player)entity).Move(WrapDirection);
                            }
                        });
                        break;
                }

                
            };

            switch (WrapDirection)
            {
                case Direction.LEFT:
                    entity.transform.DOLocalMoveX(transform.position.x - 1, 0.2f)
                        .OnComplete(()=>
                        {
                            entity.Show(false);
                            entity.transform.position = new Vector3(_wrapPos.x + 1, _wrapPos.y, entity.transform.position.z);
                            setPos();
                        });
                    break;
                case Direction.RIGHT:
                    entity.transform.DOLocalMoveX(transform.position.x + 1, 0.2f).OnComplete(() =>
                    {
                        entity.Show(false);
                        entity.transform.position = new Vector3(_wrapPos.x - 1, _wrapPos.y, entity.transform.position.z);
                        setPos();
                    });
                    break;
                case Direction.DOWN:
                    entity.transform.DOLocalMoveY(transform.position.y - 1, 0.2f).OnComplete(() =>
                    {
                        entity.Show(false);
                        entity.transform.position = new Vector3(_wrapPos.x, _wrapPos.y + 1, entity.transform.position.z);
                        setPos();
                    });
                    break;
                case Direction.UP:
                    entity.transform.DOLocalMoveY(transform.position.y + 1, 0.2f).OnComplete(() =>
                    {
                        entity.Show(false);
                        entity.transform.position = new Vector3(_wrapPos.x, _wrapPos.y - 1, entity.transform.position.z);
                        setPos();
                    });
                    break;
            }
        }

        protected override void TriggerEnterEffect(Entity entity)
        {
            containedEntity = entity;
            base.TriggerEnterEffect(entity);
            if (entity.direction != WrapDirection) return;

            StartCoroutine(Wrap(entity));
        }

        protected override void TriggerExitEffect(Entity entity)
        {
            base.TriggerExitEffect(entity);
            containedEntity = null;
        }

        Vector2 FindWrapPosition()
        {
            bool isHorizontal = false;
            int currIndex = -1;
            int motion = 0;
            int mapBound = 0;
            int xPos = (int)transform.localPosition.x;
            int yPos = (int)transform.localPosition.y;

            if (WrapDirection == Direction.RIGHT || WrapDirection == Direction.UP)
                motion = 1;
            else if (WrapDirection == Direction.LEFT || WrapDirection == Direction.DOWN)
                motion = -1;

            if (WrapDirection == Direction.LEFT || WrapDirection == Direction.RIGHT)
            {
                isHorizontal = true;
                mapBound = Services.Board.Width - 1;
                currIndex = xPos;
            }
            else if (WrapDirection == Direction.UP || WrapDirection == Direction.DOWN)
            {
                mapBound = Services.Board.Height - 1;
                currIndex = yPos;
            }

            currIndex += motion;
            for(int i = currIndex; (i - currIndex) * motion < mapBound; i+= motion)
            {
                int index = (i + mapBound) % mapBound;
                if(isHorizontal)
                {
                    if (Services.Board.Map[index, yPos].canTraverse)
                    {
                        return Services.Board.Map[index, yPos].transform.position;
                    }
                }
                else
                {
                    if (Services.Board.Map[xPos, index].canTraverse)
                    {
                        return Services.Board.Map[xPos, index].transform.position;
                    }
                }
            }
            return Vector2.negativeInfinity;
        }

    }
}