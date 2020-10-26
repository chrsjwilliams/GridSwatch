using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private TaskManager _tm = new TaskManager();

    public void CreateBaord(int w, int h)
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
