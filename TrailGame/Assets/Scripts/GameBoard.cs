using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameBoard : MonoBehaviour
{
    private int _width;
    public int Width
    {
        get { return _width; }
    }

    private int _height;
    public int Height
    {
        get { return _height; }
    }

    private Tile[,] _map;
    public Tile[,] Map
    {
        get { return _map; }
    }

    private int _emptyTileCount;
    public int EmptyTileCount { get { return _emptyTileCount; } }

    public int[] CurrentFillAmount = new int[Enum.GetNames(typeof(ColorMode)).Length];

    private TaskManager _tm = new TaskManager();

    public void CreateBoard(MapData data)
    {
        _width = (int)data.BoardSize.x;
        _height = (int)data.BoardSize.y;
        _map = new Tile[Width, Height];

        List<Vector2> allPumpLocations = new List<Vector2>();
        allPumpLocations.AddRange(data.PumpLocationsCyan);
        allPumpLocations.AddRange(data.PumpLocationsMagenta);
        allPumpLocations.AddRange(data.PumpLocationsYellow);



        _emptyTileCount = (Width * Height) - (allPumpLocations.Count + data.ImpassableMapCoords.Count);

        bool canTraverse;
        for (int x = 0; x < Width; x++)
        {
            for(int y = 0; y < Height; y++)
            {
                Vector2 candidateCoord = new Vector2(x, y);
                canTraverse = data.ImpassableMapCoords.Contains(candidateCoord) ? false : true;


                Tile newTile = Instantiate(Services.Prefabs.Tile, candidateCoord, Quaternion.identity);

                newTile.name = "Tile: [X: " + x + ", Y: " + y + "]";
                newTile.transform.parent = transform;

                

                if (allPumpLocations.Contains(candidateCoord))
                {
                    Destroy(newTile.gameObject.GetComponent<Tile>());
                    newTile.gameObject.AddComponent<PumpTile>();
                    PumpTile pumpTile = newTile.GetComponent<PumpTile>();
                    Ink pumpInk;
                    if (data.PumpLocationsCyan.Contains(candidateCoord))
                    {
                        pumpInk = new Ink(Services.ColorManager.Cyan[0], ColorMode.CYAN, int.MaxValue);
                    }
                    else if (data.PumpLocationsMagenta.Contains(candidateCoord))
                    {
                        pumpInk = new Ink(Services.ColorManager.Magenta[0], ColorMode.MAGENTA, int.MaxValue);
                    }
                    else if (data.PumpLocationsYellow.Contains(candidateCoord))
                    {
                        pumpInk = new Ink(Services.ColorManager.Yellow[0], ColorMode.YELLOW, int.MaxValue);
                    }
                    else
                    {
                        pumpInk = new Ink(Services.ColorManager.ErrorColor, ColorMode.BLACK, int.MaxValue);
                    }

                    pumpTile.Init(new MapCoord(x, y), pumpInk, canTraverse);
                    newTile.name = newTile.name + " PUMP: " + pumpInk.colorMode;
                    _map[x, y] = pumpTile;
                }
                else
                {
                    newTile.Init(new MapCoord(x, y), new Ink(canTraverse), canTraverse);
                    _map[x, y] = newTile;
                }

            }
        }
    }

    public void CreateBoard(int w, int h)
    {
        _width = w;
        _height = h;
        _map = new Tile[Width, Height];
        Ink initInk;
        
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Tile newTile = Instantiate(Services.Prefabs.Tile, new Vector2(x, y), Quaternion.identity);
                if (x == 0 && y == 2)
                {
                    Destroy(newTile.gameObject.GetComponent<Tile>());
                    newTile.gameObject.AddComponent<PumpTile>();
                }
                newTile.name = "Tile: [ X: " + x + ", Y: " + y + "]";
                newTile.transform.parent = transform;
                if (x == 0 && y == 2)
                {
                    PumpTile pumpTile = newTile.GetComponent<PumpTile>();
                    Ink pumpInk = new Ink(Services.ColorManager.Colors[0][0], ColorMode.CYAN, int.MaxValue);

                    pumpTile.Init(new MapCoord(x, y), pumpInk, true);
                    _map[x, y] = pumpTile;
                }
                else
                {

                    initInk = new Ink();

                    newTile.Init(new MapCoord(x, y), initInk);
                    _map[x, y] = newTile;
                }
            }
        }

    }



    public bool ContainsCoord(MapCoord candidateCoord)
    {
        return  0 <= candidateCoord.x && candidateCoord.x < Width &&
                0 <= candidateCoord.y && candidateCoord.y < Height;
    }

    // Update is called once per frame
    void Update()
    {
        _tm.Update();
    }
}
