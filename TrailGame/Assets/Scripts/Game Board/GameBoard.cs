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
                    MapCoord coord = new MapCoord(x, y);
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
                        newTile.Init(coord, new Ink(canTraverse), canTraverse);
                        _map[x, y] = newTile;
                        continue;
                    }

                    Ink ink = new Ink(canTraverse: true);
                    if (tileData.hasCustomInk)
                    {
                        ink = new Ink(tileData.ink.colorMode);
                    }

                    if (tileData.isPumpTile)
                    {
                        newTile.gameObject.AddComponent<PumpTile>();
                        PumpTile pumpTile = newTile.GetComponent<PumpTile>();
                        pumpTile.Init(coord, newTile, ink, canTraverse);
                        newTile.name = newTile.name + " | PUMP: " + ink.colorMode;
                        _map[x, y] = pumpTile;
                    }

                    if (tileData.isPivotTile)
                    {
                        newTile.gameObject.AddComponent<PivotTile>();
                        PivotTile pivotTile = newTile.GetComponent<PivotTile>();
                        pivotTile.Init(coord, newTile, ink, true, tileData.PivotDirection);
                        newTile.name = newTile.name + " | PIVOT: " + tileData.PivotDirection.ToString();
                        _map[x, y] = pivotTile;
                    }

                    if(tileData.isWrapTile)
                    {
                        newTile.gameObject.AddComponent<WrapTile>();
                        WrapTile wrapTile = newTile.GetComponent<WrapTile>();
                        wrapTile.Init(coord, newTile, ink, tileData.WrapDirection);
                        newTile.name = newTile.name + " | WRAP: " + tileData.WrapDirection.ToString();
                        _map[x, y] = wrapTile;
                    }

                    if(tileData.isInvertTile)
                    {
                        newTile.gameObject.AddComponent<InvertTile>();
                        InvertTile invertTile = newTile.GetComponent<InvertTile>();
                        invertTile.Init(coord, newTile, ink);
                        newTile.name = newTile.name + " | Invert";
                        _map[x, y] = invertTile;
                    }

                    if(tileData.isFillTile)
                    {
                        newTile.gameObject.AddComponent<FillTile>();
                        FillTile fillTile = newTile.GetComponent<FillTile>();
                        fillTile.Init(coord, newTile, ink, tileData.fillType, tileData.fillColor);
                        newTile.name = newTile.name + " | Fill: " + tileData.fillType.ToString();
                        _map[x, y] = fillTile;
                    }
                }
            }
        }

        public List<Tile> GetAllAdjacentTiles(Tile tile)
        {
            List<Tile> adjacentTiles = new List<Tile>();
            List<MapCoord> candidateAdjacentCoords = tile.Coord.GetAllAdjacentCoords();
            foreach(MapCoord coord in candidateAdjacentCoords)
            {
                if (!ContainsCoord(coord)) continue;
                Tile candidateTile = _map[coord.x, coord.y];
                if (candidateTile.canTraverse)
                {
                    adjacentTiles.Add(_map[coord.x, coord.y]);
                }
            }

            return adjacentTiles;
        }

        public List<Tile> GetAdjacentHorizontalTiles(Tile tile)
        {
            List<Tile> adjacentHorizontalTiles = new List<Tile>();
            List<MapCoord> candidateAdjacentHorizontalCoords = tile.Coord.GetAdjacentHorizontalCoords();
            foreach (MapCoord coord in candidateAdjacentHorizontalCoords)
            {
                if (!ContainsCoord(coord)) continue;
                Tile candidateTile = _map[coord.x, coord.y];
                if (candidateTile.canTraverse)
                {
                    adjacentHorizontalTiles.Add(_map[coord.x, coord.y]);
                }
            }

            return adjacentHorizontalTiles;
        }

        public List<Tile> GetAdjacentVerticleTiles(Tile tile)
        {
            List<Tile> adjacentVerticleTiles = new List<Tile>();
            List<MapCoord> candidateAdjacentVerticleCoords = tile.Coord.GetAdjacentVerticleCoords();
            foreach (MapCoord coord in candidateAdjacentVerticleCoords)
            {
                if (!ContainsCoord(coord)) continue;
                Tile candidateTile = _map[coord.x, coord.y];
                if (candidateTile.canTraverse)
                {
                    adjacentVerticleTiles.Add(_map[coord.x, coord.y]);
                }
            }

            return adjacentVerticleTiles;
        }

        public List<Tile> GetRowOf(Tile tile)
        {
            List<Tile> rowTiles = new List<Tile>();

            for(int i = 0; i < Width; i++)
            {
                Tile candidateTile = _map[i, tile.Coord.y];
                if (tile.canTraverse)
                {
                    rowTiles.Add(candidateTile);
                }
            }

            return rowTiles;
        }

        public List<Tile> GetColumnOf(Tile tile)
        {
            List<Tile> columnTiles = new List<Tile>();
            for (int i = 0; i < Height; i++)
            {
                Tile candidateTile = _map[tile.Coord.x, i];
                if (tile.canTraverse)
                {
                    columnTiles.Add(candidateTile);
                }
                else
                {
                    Debug.Log(tile.Coord.ToString());
                }
            }
            return columnTiles;
        }

        public List<Tile> GetInterCardinalOf(Tile tile)
        {
            List<Tile> intercardinalTiles = new List<Tile>();
            List<MapCoord> candidateIntercardinalCoords = tile.Coord.GetAdjacentIntercardinalCoords();
            foreach (MapCoord coord in candidateIntercardinalCoords)
            {
                if (!ContainsCoord(coord)) continue;

                intercardinalTiles.Add(_map[coord.x, coord.y]);
            }
            return intercardinalTiles;
        }

        public List<Tile> GetCardinalOf(Tile tile)
        {
            List<Tile> cardinalTiles = new List<Tile>();
            List<MapCoord> candidateCardinalCoords = tile.Coord.GetAdjacentCardinalCoords();
            foreach (MapCoord coord in candidateCardinalCoords)
            {
                if (!ContainsCoord(coord)) continue;

                cardinalTiles.Add(_map[coord.x, coord.y]);
            }
            return cardinalTiles;
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