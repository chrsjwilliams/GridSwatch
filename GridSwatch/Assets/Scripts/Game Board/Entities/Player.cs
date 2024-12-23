﻿using System.Collections.Generic;
using UnityEngine;
using GameData;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.ComTypes;

public class Player : Entity
{
    public const int FULL_INTENSITY_SWIPES = 2;
    public const int DIM_INTENSITY_SWIPES = 1;

    public const int MAX_INTENSITY_LEVEL = 2;

    public int fullIntensitySwipeCount;
    public int dimIntensitySwipeCount;

    public bool isMoving;

    
    [SerializeField] CanvasGroup indicatorGroup;
    [SerializeField] List<Image> colorIndicators;
    public ReadOnlyCollection<Image> ColorIndicators
    {
        get { return colorIndicators.AsReadOnly(); }
    }

    private List<Vector3> playerPoints;
    public override void Init(MapCoord c)
    {
        receiveInput = true;
        isMoving = false;
        Ink = new Ink(ColorMode.NONE);
        canMove = true;
        coord = c;
        SetPosition(coord);
        direction = Direction.NONE;
        Services.EventManager.Register<SwipeEvent>(OnSwipe);
        moveSpeed = 1.5f;
        arriveSpeed = 1;
        ResetIntensitySwipes();
        CurrentColorMode = Ink.colorMode;
        playerPoints = new List<Vector3>();
        playerPoints.Add(new Vector3(coord.x, coord.y));
    }

    public override void PivotDirection(Direction d)
    {
        direction = d;
    }

    private void OnDestroy()
    {
        Services.EventManager.Unregister<SwipeEvent>(OnSwipe);
    }

    public override void Show(bool show)
    {
        Sprite.color = show ? Color.black : Color.clear;
        indicatorGroup.alpha = show ? 1 : 0;
    }

    public override void ResetIntensitySwipes()
    {
        Ink.Intensity = MAX_INTENSITY_LEVEL;
        fullIntensitySwipeCount = swipeCount = FULL_INTENSITY_SWIPES;
        dimIntensitySwipeCount = DIM_INTENSITY_SWIPES;
    }

    public override void SetIndicators(Color color, int swipes  = 0)
    {
        if (colorIndicators == null) return;

        for (int i = colorIndicators.Count - 1; i >=  swipes; i--)
        {
            colorIndicators[i].DOColor(color, 0.33f).SetEase(Ease.InCubic);
        }
    }

    void UseInidcator(int index)
    {
        int i = Math.Abs(colorIndicators.Count - index);
        colorIndicators[i].DOColor(Color.white, 0.33f).SetEase(Ease.InCubic);
    }


    public Color GetColor()
    {
        if (CurrentColorMode == ColorMode.NONE) return Color.clear;
        return Services.ColorManager.ColorScheme.GetColor(CurrentColorMode)[0];
    }

    public void SetPosition(MapCoord c)
    {
        transform.position = new Vector3(c.x, c.y);
    }

    protected void OnSwipe(SwipeEvent e)
    {
        if (!receiveInput) return;

        if (swipeCount <= 0 && CurrentColorMode != ColorMode.NONE)
        {
            Ink.Intensity = 0;
            CurrentColorMode = ColorMode.NONE;
            swipeCount = 0;
        }
        
        if (CurrentColorMode != ColorMode.NONE && swipeCount > 0)
        {
            UseInidcator(swipeCount);
            swipeCount--;
        }
        direction = e.gesture.CurrentDirection;

        
        float xPos = Mathf.Round(transform.position.x);
        float yPos = Mathf.Round(transform.position.y);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }

    public void Move(Direction dir)
    {
        if (dir == Direction.NONE) return;

        MapCoord deltaPos;
        switch (dir)
        {
            case Direction.LEFT:
                deltaPos = MapCoord.LEFT;
                break;
            case Direction.RIGHT:
                deltaPos = MapCoord.RIGHT;
                break;
            case Direction.UP:
                deltaPos = MapCoord.UP;
                break;
            case Direction.DOWN:
                deltaPos = MapCoord.DOWN;
                break;
            default:
                deltaPos = MapCoord.ZERO;
                Debug.LogError("ERROR : Invalid Direction");
                break;
        }

        MapCoord candidateCoord = MapCoord.Add(coord, deltaPos);
        if (CanTraverse(candidateCoord))
        {
            Vector3 movePos = new Vector3(deltaPos.x, deltaPos.y);
            transform.position +=
                movePos * Time.deltaTime * moveSpeed * Services.GameManager.MainCamera.orthographicSize;
            int xPos = (int)Mathf.Floor(transform.localPosition.x);
            int yPos = (int)Mathf.Floor(transform.localPosition.y);
            coord = new MapCoord(xPos, yPos);
            if (CurrentColorMode != ColorMode.NONE)
            {
                Ink.color = GetColor();
            }

            isMoving = true;
            playerPoints[playerPoints.Count - 1] = transform.localPosition;
        }
        else if(!CanTraverse(candidateCoord) && isMoving)
        {
            float xArriveMod = direction == Direction.LEFT ? 15f : 15f;
            float yArriveMod = direction == Direction.DOWN ? 15f : 15f;


            float xPos = Mathf.Lerp(transform.localPosition.x, (int)transform.localPosition.x,
                Time.deltaTime * arriveSpeed * xArriveMod);
            float yPos = Mathf.Lerp(transform.localPosition.y, (int)transform.localPosition.y,
                Time.deltaTime * arriveSpeed * yArriveMod);
            // check direction and stop 1 before that candidate coord

            switch (dir)
            {
                case Direction.LEFT:
                    if (xPos > Services.Board.Map[candidateCoord.x + 1, candidateCoord.y].transform.localPosition.x)
                    {
                        xPos = Services.Board.Map[candidateCoord.x + 1, candidateCoord.y].transform.localPosition.x;
                    }

                    break;
                case Direction.RIGHT:
                    if (xPos > Services.Board.Map[candidateCoord.x - 1, candidateCoord.y].transform.localPosition.x)
                    {
                        xPos = Services.Board.Map[candidateCoord.x - 1, candidateCoord.y].transform.localPosition.x;
                    }

                    break;
                case Direction.UP:
                    if (yPos > Services.Board.Map[candidateCoord.x, candidateCoord.y - 1].transform.localPosition.y)
                    {
                        yPos = Services.Board.Map[candidateCoord.x, candidateCoord.y - 1].transform.localPosition.y;
                    }

                    break;
                case Direction.DOWN:
                    
                    if (yPos > Services.Board.Map[candidateCoord.x, candidateCoord.y + 1].transform.localPosition.y)
                    {
                        yPos = Services.Board.Map[candidateCoord.x, candidateCoord.y + 1].transform.localPosition.y;
                    }
                    break;
                default:
                    deltaPos = MapCoord.ZERO;
                    Debug.LogError("ERROR : Invalid Direction");
                    break;
            }

            Vector3 newPosition = new Vector3(xPos, yPos, transform.localPosition.z);
            coord = new MapCoord(xPos, yPos);

            isMoving = false;
            if (CurrentColorMode != ColorMode.NONE)
            {
                Ink.color = GetColor();
            }
            transform.DOLocalMove(newPosition, 0.07f).SetEase(Ease.InCirc).OnComplete(() =>
            {
            });
        }
    }

    private bool CanTraverse(MapCoord candidateCoord)
    {
        bool canTraverse = Services.GameScene.board.ContainsCoord(candidateCoord) &&
                           Services.GameScene.board.Map[candidateCoord.x, candidateCoord.y].GetCanTraverse(this);

        return canTraverse;
    }

    private void Update()
    {
        if (canMove)
        {
            Move(direction);
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
}