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

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Tile newTile = Instantiate(Services.Prefabs.Tile, new Vector2(x, y), Quaternion.identity);
                newTile.name = "Tile: [ X: " + x + ", Y: " + y + "]";

                newTile.transform.parent = transform;
                newTile.Init();
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        _tm.Update();
    }
}
