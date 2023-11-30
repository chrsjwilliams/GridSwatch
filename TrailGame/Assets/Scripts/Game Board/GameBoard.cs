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

        public void CreateBoard(MapData data)
        {
            _width = (int)data.BoardSize.x;
            _height = (int)data.BoardSize.y;
            _map = new Tile[Width, Height];
            _boardType = data.BoardType;


            _emptyTileCount = (Width * Height) - (data.tileData.Count + data.ImpassableMapCoords.Count);

            bool canTraverse;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    // Create coordinate
                    Vector2 candidateCoord = new Vector2(x, y);
                    canTraverse = data.ImpassableMapCoords.Contains(candidateCoord) ? false : true;

                    // Create Tile
                    Tile newTile = Instantiate(Services.Prefabs.Tile, candidateCoord, Quaternion.identity);
                    newTile.name = "Tile: [X: " + x + ", Y: " + y + "]";
                    newTile.transform.parent = transform;


                    TileData tileData = data.GetTile(candidateCoord);

                    // If we have no other tle data, move to next tile
                    if(tileData.coord.x == -1)
                    {
                        newTile.Init(new Ink(canTraverse), canTraverse);
                        _map[x, y] = newTile;
                        continue;
                    }

                    Ink ink = new Ink();
                    if (tileData.hasCustomInk)
                    {
                        ink = new Ink(tileData.ink.colorMode);
                    }

                    if (tileData.isPumpTile)
                    {
                        newTile.gameObject.AddComponent<PumpTile>();
                        PumpTile pumpTile = newTile.GetComponent<PumpTile>();
                        pumpTile.Init(newTile, ink, canTraverse);
                        newTile.name = newTile.name + " | PUMP: " + ink.colorMode;
                        _map[x, y] = pumpTile;
                    }

                    if (tileData.isPivotTile)
                    {
                        newTile.gameObject.AddComponent<PivotTile>();
                        PivotTile pivotTile = newTile.GetComponent<PivotTile>();
                        pivotTile.Init(newTile, ink, true, tileData.PivotDirection);
                        newTile.name = newTile.name + " | PIVOT: " + tileData.PivotDirection.ToString();
                        _map[x, y] = pivotTile;
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
    }
}