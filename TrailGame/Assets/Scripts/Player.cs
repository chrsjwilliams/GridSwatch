using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    /*
     *  TODO:
     *          Have color be determined by ColorMode
     *          Level design pipline? csv sheets?
     * 
     */ 
    public override void Init(MapCoord c)
    {
        CurrentColorMode = ColorMode.NONE;
        canMove = true;
        coord = c;
        SetPosition(coord);
        direction = Swipe.Direction.NONE;
        Services.EventManager.Register<SwipeEvent>(OnSwipe);
        moveSpeed = 2;
        arriveSpeed = 10;
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
            transform.position += movePos * Time.deltaTime * moveSpeed;
            int xPos = (int)Mathf.Floor(transform.position.x);
            int yPos = (int)Mathf.Floor(transform.position.y);
            coord = new MapCoord(xPos, yPos);

            Tile currentTile = Services.GameScene.board.Map[coord.x, coord.y];
            currentTile.SetColor(Color.magenta);
        }
        else
        {
            float xArriveMod = direction == Swipe.Direction.LEFT ? 0.33f : 1;
            float yArriveMod = direction == Swipe.Direction.DOWN ? 0.33f : 1;


            float xPos = Mathf.Lerp(transform.position.x,(int)transform.position.x, Time.deltaTime * arriveSpeed * xArriveMod);
            float yPos = Mathf.Lerp(transform.position.y,(int)transform.position.y, Time.deltaTime * arriveSpeed * yArriveMod);
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
                 e.gesture.CurrentDirection == Swipe.Direction.RIGHT));
    }

}
