using UnityEngine;
using GameData;

public class Player : Entity
{
    public const int FULL_INTENSITY_SWIPES = 2;
    public const int DIM_INTENSITY_SWIPES = 1;

    public const int MAX_INTENSITY_LEVEL = 2;

    public int fullIntensitySwipeCount;
    public int dimIntensitySwipeCount;

    public bool isMoving;

    [SerializeField] int swipeCount = 0;

    [SerializeField] SpriteRenderer colorIndicator;

    public override void Init(MapCoord c)
    {
        isMoving = false;
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

    private void OnDestroy()
    {
        Services.EventManager.Unregister<SwipeEvent>(OnSwipe);
    }

    public void ResetIntensitySwipes()
    {
        Ink.Intensity = MAX_INTENSITY_LEVEL;
        fullIntensitySwipeCount = swipeCount = FULL_INTENSITY_SWIPES;
        dimIntensitySwipeCount = DIM_INTENSITY_SWIPES;
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
            colorIndicator.color = Services.ColorManager.ColorScheme.GetColor(CurrentColorMode)[intensityIndex];
        }

        if (Ink.Intensity == 0 || (swipeCount == 0 && Services.Board.BoardType == GameBoard.ColorType.MARKER))
        {
            Ink.Intensity = 0;
            CurrentColorMode = ColorMode.NONE;
            colorIndicator.color = Color.white;
            swipeCount = 0;
        }
        if (CurrentColorMode != ColorMode.NONE)
        {
            swipeCount--;
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
            isMoving = true;
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
            Vector3 newPosition = new Vector3(xPos, yPos, transform.position.z);
            isMoving = false;

            transform.position = newPosition;

        }

        //isMoving = new Vector3((int)transform.position.x, (int)transform.position.y) == new Vector3(candidateCoord.x, candidateCoord.y);
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
        return ((direction == Swipe.Direction.LEFT || direction == Swipe.Direction.RIGHT) &&
                (e.gesture.CurrentDirection == Swipe.Direction.UP || e.gesture.CurrentDirection == Swipe.Direction.DOWN)) ||
               ((direction == Swipe.Direction.UP || direction == Swipe.Direction.DOWN) &&
                (e.gesture.CurrentDirection == Swipe.Direction.LEFT || e.gesture.CurrentDirection == Swipe.Direction.RIGHT)) ||
                (direction == Swipe.Direction.NONE &&e.gesture.CurrentDirection != Swipe.Direction.NONE);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.GetComponent<Tile>();
        if (tile == null) return;
        if (tile is PumpTile)
        {
            Ink = ((PumpTile)tile).tileInk;
            CurrentColorMode = Ink.colorMode;
            colorIndicator.color = Services.ColorManager.ColorScheme.GetColor(CurrentColorMode)[0];
            ResetIntensitySwipes();
        }
        else if(CurrentColorMode != ColorMode.NONE &&
            dimIntensitySwipeCount > 0 &&
            tile.canTraverse &&
            tile.CurrentColorMode == ColorMode.NONE)
        {
            Ink.color = GetColor();
            tile.SetColor(Ink);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Tile tile = collision.GetComponent<Tile>();
        if (tile == null) return;
        if (tile is PumpTile) return;
        // This allows us to pass before potentially setting a tile to Black
        if (CurrentColorMode != ColorMode.NONE &&
            dimIntensitySwipeCount > 0 &&
            tile.canTraverse &&
            tile.CurrentColorMode != ColorMode.NONE)
        {
            Ink.color = GetColor();
            tile.SetColor(Ink);
        }
    }

}
