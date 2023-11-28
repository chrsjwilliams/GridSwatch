using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameData
{
    public class GameBoard : MonoBehaviour
    {
        public enum ColorType { MARKER, BRUSH}

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

        private ColorType _boardType;
        public ColorType BoardType { get { return _boardType; } }

        private int _emptyTileCount;
        public int EmptyTileCount { get { return _emptyTileCount; } }

        public int[] CurrentFillAmount = new int[Enum.GetNames(typeof(ColorMode)).Length];

        private TaskManager _tm = new TaskManager();

        public void CreateBoard(MapData data)
        {
            _width = (int)data.BoardSize.x;
            _height = (int)data.BoardSize.y;
            _map = new Tile[Width, Height];
            _boardType = data.BoardType;

            List<Vector2> allPumpLocations = new List<Vector2>();
            allPumpLocations.AddRange(data.PumpLocationsCyan);
            allPumpLocations.AddRange(data.PumpLocationsMagenta);
            allPumpLocations.AddRange(data.PumpLocationsYellow);



            _emptyTileCount = (Width * Height) - (allPumpLocations.Count + data.ImpassableMapCoords.Count);

            bool canTraverse;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Vector2 candidateCoord = new Vector2(x, y);
                    canTraverse = data.ImpassableMapCoords.Contains(candidateCoord) ? false : true;


                    Tile newTile = Instantiate(Services.Prefabs.Tile, candidateCoord, Quaternion.identity);

                    newTile.name = "Tile: [X: " + x + ", Y: " + y + "]";
                    newTile.transform.parent = transform;


                    if (data.HasTileData(candidateCoord))
                    {

                        TileData tileData = data.GetTile(candidateCoord);
                        Ink ink = new Ink();
                        if (tileData.hasCustomInk)
                        {
                            ink = new Ink(tileData.ink.colorMode);
                        }

                        if (tileData.isPumpTile)
                        {
                            Destroy(newTile.gameObject.GetComponent<Tile>());
                            newTile.gameObject.AddComponent<PumpTile>();
                            PumpTile pumpTile = newTile.GetComponent<PumpTile>();
                            pumpTile.Init(new MapCoord(x, y), ink, canTraverse);
                            newTile.name = newTile.name + " PUMP: " + ink.colorMode;
                            _map[x, y] = pumpTile;
                        }

                        if (tileData.isPivotTile)
                        {
                            newTile.pivotDirection = tileData.PivotDirection;
                            newTile.name = newTile.name + " | PIVOT: " + newTile.pivotDirection.ToString();
                            newTile.Init(new MapCoord(x, y), ink, canTraverse);
                            _map[x, y] = newTile;
                        }
                    }
                    //if (allPumpLocations.Contains(candidateCoord))
                    //{
                    //    Destroy(newTile.gameObject.GetComponent<Tile>());
                    //    newTile.gameObject.AddComponent<PumpTile>();
                    //    PumpTile pumpTile = newTile.GetComponent<PumpTile>();
                    //    Ink pumpInk;
                    //    if (data.PumpLocationsCyan.Contains(candidateCoord))
                    //    {
                    //        pumpInk = new Ink(Services.ColorManager.ColorScheme.GetColor(ColorMode.CYAN)[0], ColorMode.CYAN, int.MaxValue);
                    //    }
                    //    else if (data.PumpLocationsMagenta.Contains(candidateCoord))
                    //    {
                    //        pumpInk = new Ink(Services.ColorManager.ColorScheme.GetColor(ColorMode.MAGENTA)[0], ColorMode.MAGENTA, int.MaxValue);
                    //    }
                    //    else if (data.PumpLocationsYellow.Contains(candidateCoord))
                    //    {
                    //        pumpInk = new Ink(Services.ColorManager.ColorScheme.GetColor(ColorMode.YELLOW)[0], ColorMode.YELLOW, int.MaxValue);
                    //    }
                    //    else
                    //    {
                    //        pumpInk = new Ink(Services.ColorManager.ColorScheme.ErrorColor, ColorMode.BLACK, int.MaxValue);
                    //    }

                    //    pumpTile.Init(new MapCoord(x, y), pumpInk, canTraverse);
                    //    newTile.name = newTile.name + " PUMP: " + pumpInk.colorMode;
                    //    _map[x, y] = pumpTile;
                    //}
                    else
                    {
                        newTile.Init(new MapCoord(x, y), new Ink(canTraverse), canTraverse);
                        _map[x, y] = newTile;
                    }

                }
            }
        }

        public void ResetMap()
        {

            for(int i = 0; i < CurrentFillAmount.Length; i++)
            {
                CurrentFillAmount[i] = 0;
            }

            List<Tile> tilesToDelete = new List<Tile>();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    tilesToDelete.Add(_map[x, y]);
                }
            }

            foreach(Tile tile in tilesToDelete)
            {
                Destroy(tile.gameObject);
            }

        }

        public bool ContainsCoord(MapCoord candidateCoord)
        {
            return 0 <= candidateCoord.x && candidateCoord.x < Width &&
                    0 <= candidateCoord.y && candidateCoord.y < Height;
        }

        // Update is called once per frame
        void Update()
        {
            _tm.Update();
        }
    }
}