using System.Collections.Generic;
using UnityEngine;
using GameData;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System;
using System.Runtime.InteropServices.ComTypes;

public class Player : Entity
{
    public const int FULL_INTENSITY_SWIPES = 2;
    public const int DIM_INTENSITY_SWIPES = 1;

    public const int MAX_INTENSITY_LEVEL = 2;

    public int fullIntensitySwipeCount;
    public int dimIntensitySwipeCount;

    public bool isMoving;

    [SerializeField] int swipeCount = 0;
    [SerializeField] CanvasGroup indicatorGroup;
    [SerializeField] List<Image> colorIndicators;
    
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
        moveSpeed = 2;
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

    public override void SetIndicators(Color color)
    {
        if (colorIndicators == null) return;

        foreach (var inidcator in colorIndicators)
        {
            inidcator.DOColor(color, 0.33f).SetEase(Ease.InCubic);
        }

    }

    void UseInidcator(int index)
    {
        colorIndicators[index].DOColor(Color.white, 0.33f).SetEase(Ease.InCubic);
    }


    public Color GetColor()
    {
        if (CurrentColorMode == ColorMode.NONE) return Color.clear;
        
        int intensityIndex = -1;

        if (Ink.Intensity > 1) intensityIndex = (int)ColorManager.Intensity.FULL;
        else if (Ink.Intensity > 0) intensityIndex = (int)ColorManager.Intensity.DIM;
        else return Color.clear;

        return Services.ColorManager.ColorScheme.GetColor(CurrentColorMode)[intensityIndex];
    }

    public void SetPosition(MapCoord c)
    {
        transform.position = new Vector3(c.x, c.y);
    }

    protected void OnSwipe(SwipeEvent e)
    {
        if (!receiveInput)
        {
            if (CurrentColorMode != ColorMode.NONE && swipeCount > 0)
            {
                UseInidcator(swipeCount - 1);
                swipeCount--;
            }

            return;
        }

        // removing axis swipe becase it breaks marker mode
        // if i wnt levels of color i need to be able to detect when i move orthonogally(?)
        // no, it's more like, if the color below me is also my colortype, then i shouldn't lose
        // power
        float xPos = Mathf.Round(transform.position.x);
        float yPos = Mathf.Round(transform.position.y);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
        int intensityIndex = -1;
        if (Services.Board.BoardType == GameBoard.ColorType.BRUSH)
        {
            Ink.Intensity--;
        }

        if (Ink.Intensity > 1) intensityIndex = (int)ColorManager.Intensity.FULL;
        else if (Ink.Intensity > 0) intensityIndex = (int)ColorManager.Intensity.DIM;

        if (intensityIndex != -1 && CurrentColorMode != ColorMode.NONE)
        {
            //colorIndicator.color = Services.ColorManager.ColorScheme.GetColor(CurrentColorMode)[intensityIndex];
        }

        if (Ink.Intensity == 0 || (swipeCount == 0 && Services.Board.BoardType == GameBoard.ColorType.MARKER))
        {
            Ink.Intensity = 0;
            CurrentColorMode = ColorMode.NONE;
            swipeCount = 0;
        }

        // We update swipe counts here since some tile effects
        // stop the player from receiving input
        if (CurrentColorMode != ColorMode.NONE)
        {
            
            UseInidcator(swipeCount - 1);
            swipeCount--;
        }

        direction = e.gesture.CurrentDirection;

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
            Tile currentTile = Services.GameScene.board.Map[candidateCoord.x, candidateCoord.y]; 
            if (CurrentColorMode != ColorMode.NONE && dimIntensitySwipeCount > 0)
            {
                Ink.color = GetColor();
            }

            isMoving = true;
            playerPoints[playerPoints.Count - 1] = transform.localPosition;
        }
        else
        {
            float xArriveMod = direction == Direction.LEFT ? 15f : 1;
            float yArriveMod = direction == Direction.DOWN ? 15f : 1f;


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
            isMoving = false;
            transform.DOLocalMove(newPosition, 0.01f).SetEase(Ease.InCirc);
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