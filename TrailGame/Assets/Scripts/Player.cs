using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public const int FULL_INTENSITY_SWIPES = 2;
    public const int DIM_INTENSITY_SWIPES = 1;

    public const int MAX_INTENSITY_LEVEL = 3;

    public int fullIntensitySwipeCount;
    public int dimIntensitySwipeCount;

    public override void Init(MapCoord c)
    {
        
        Ink = new Ink(ColorMode.NONE);        
        canMove = true;
        coord = c;
        SetPosition(coord);
        direction = Swipe.Direction.NONE;
        Services.EventManager.Register<SwipeEvent>(OnSwipe);
        moveSpeed = 2;
        arriveSpeed = 1;
        ResetIntensitySwipes();
        CurrentColorMode = Ink.colorMode;
    }

    public void ResetIntensitySwipes()
    {
        Ink.Intensity = MAX_INTENSITY_LEVEL;
        fullIntensitySwipeCount = FULL_INTENSITY_SWIPES;
        dimIntensitySwipeCount = DIM_INTENSITY_SWIPES;
    }


    public Color GetColor()
    {
        if (CurrentColorMode == ColorMode.NONE) return Color.clear;

        int intensityIndex = -1;

        if (Ink.Intensity > 1) intensityIndex = (int)ColorManager.Intensity.FULL;
        else if (Ink.Intensity > 0) intensityIndex = (int)ColorManager.Intensity.DIM;
        else return Color.clear;


        return Services.ColorManager.Colors[(int)CurrentColorMode - 1][intensityIndex];
    }

    public void SetPosition(MapCoord c)
    {
        transform.position = new Vector3(c.x, c.y);
    }

    protected void OnSwipe(SwipeEvent e)
    {
        if(AxisSwipeChange(e))
        {
            float xPos = Mathf.Round(transform.position.x);
            float yPos = Mathf.Round(transform.position.y);
            transform.position = new Vector3(xPos, yPos, transform.position.z);
            Ink.Intensity--;
            if (Ink.Intensity == 0)
            {
                Ink.Intensity = 0;
                CurrentColorMode = ColorMode.NONE;
            }

        }
        direction = e.gesture.CurrentDirection;

    }

    protected void Move(Swipe.Direction dir)
    {
        if(dir == Swipe.Direction.NONE) return;

        MapCoord deltaPos;
        switch(dir)
        {
            case Swipe.Direction.LEFT:
                deltaPos = MapCoord.LEFT;
                break;
            case Swipe.Direction.RIGHT:
                deltaPos = MapCoord.RIGHT;
                break;
            case Swipe.Direction.UP:
                deltaPos = MapCoord.UP;
                break;
            case Swipe.Direction.DOWN:
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
            transform.position += movePos * Time.deltaTime * moveSpeed * Services.GameManager.MainCamera.orthographicSize;
            int xPos = (int)Mathf.Floor(transform.position.x);
            int yPos = (int)Mathf.Floor(transform.position.y);
            coord = new MapCoord(xPos, yPos);

            Tile currentTile = Services.GameScene.board.Map[candidateCoord.x, candidateCoord.y];
            if (currentTile is PumpTile)
            {
                
            }
            else if (CurrentColorMode != ColorMode.NONE && dimIntensitySwipeCount > 0)
            {
                Ink.color = GetColor();

                // This should nly happen when I enter a tile
                //currentTile.SetColor(Ink);
            }
        }
        else
        {
            float xArriveMod = direction == Swipe.Direction.LEFT ? 15f : 1;
            float yArriveMod = direction == Swipe.Direction.DOWN ? 15f : 1f;


            float xPos = Mathf.Lerp(transform.position.x,(int)transform.position.x, Time.deltaTime * arriveSpeed * xArriveMod);
            float yPos = Mathf.Lerp(transform.position.y,(int)transform.position.y, Time.deltaTime * arriveSpeed * yArriveMod);
            // check direction and stop 1 before that candidate coord

            switch(dir)
            {
                case Swipe.Direction.LEFT:
                    if(xPos < Services.Board.Map[candidateCoord.x + 1, candidateCoord.y].transform.position.x){
                        xPos = Services.Board.Map[candidateCoord.x + 1, candidateCoord.y].transform.position.x;
                    }
                break;
            case Swipe.Direction.RIGHT:
                if(xPos > Services.Board.Map[candidateCoord.x - 1, candidateCoord.y].transform.position.x){
                        xPos = Services.Board.Map[candidateCoord.x - 1, candidateCoord.y].transform.position.x;
                    }
                break;
            case Swipe.Direction.UP:
                if(yPos > Services.Board.Map[candidateCoord.x, candidateCoord.y - 1].transform.position.y){
                        yPos = Services.Board.Map[candidateCoord.x, candidateCoord.y - 1].transform.position.y;
                    }
                
                break;
            case Swipe.Direction.DOWN:
                if(yPos < Services.Board.Map[candidateCoord.x,  candidateCoord.y + 1].transform.position.y){
                        yPos = Services.Board.Map[candidateCoord.x, candidateCoord.y + 1].transform.position.y;
                    }
                break;
            default:
                deltaPos = MapCoord.ZERO;
                Debug.LogError("ERROR : Invalid Direction");
                break;
            }
            transform.position = new Vector3(xPos, yPos, transform.position.z);
        }
    }
    // Have a method that gives me the direction I should move!

    private bool CanTraverse(MapCoord candidateCoord)
    {
        bool canTraverse = false;
        if( Services.GameScene.board.ContainsCoord(candidateCoord) &&
            Services.GameScene.board.Map[candidateCoord.x, candidateCoord.y].canTraverse)
            canTraverse = true;

        return canTraverse;

    }

    private void Update() {
        if(canMove)
        {
            Move(direction);
        }
    }

    public bool AxisSwipeChange(SwipeEvent e)
    {
        return ((direction == Swipe.Direction.LEFT ||
                 direction == Swipe.Direction.RIGHT) &&
                (e.gesture.CurrentDirection == Swipe.Direction.UP ||
                 e.gesture.CurrentDirection == Swipe.Direction.DOWN)) ||
               ((direction == Swipe.Direction.UP ||
                 direction == Swipe.Direction.DOWN) &&
                (e.gesture.CurrentDirection == Swipe.Direction.LEFT ||
                 e.gesture.CurrentDirection == Swipe.Direction.RIGHT)) ||
                (direction == Swipe.Direction.NONE &&
                e.gesture.CurrentDirection != Swipe.Direction.NONE);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.GetComponent<Tile>();
        if(tile != null)
        {

            if (tile is PumpTile)
            {
                Ink = ((PumpTile)tile).tileInk;
                CurrentColorMode = Ink.colorMode;
                ResetIntensitySwipes();
            }
            if (CurrentColorMode != ColorMode.NONE && dimIntensitySwipeCount > 0 && tile.canTraverse)
            {
                Ink.color = GetColor();

                // This should only happen when I enter a tile
                tile.SetColor(Ink);
           }

        }
    }

}
