using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GameData
{
    [CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/MapData", order = 1)]
    public class MapData : ScriptableObject
    {
        public string mapName;
        public GameBoard.ColorType BoardType;
        public Vector2 BoardSize;

        public List<Vector2> ImpassableMapCoords;

        public Vector2 PlayerStartPos;

        public List<Vector2> PumpLocationsMagenta;
        public List<Vector2> PumpLocationsCyan;
        public List<Vector2> PumpLocationsYellow;


        public string MapGoal;

        public List<ColorGoal> colorGoals;

        public List<TileData> tileData;

        [System.Serializable]
        public struct ColorGoal
        {
            public ColorMode colorMode;
            public int amount;
        }

        public bool HasTileData(Vector2 coord)
        {
            foreach(TileData tile in tileData)
            {
                if (tile.coord == coord)
                    return true;
            }

            return false;
        }

        public TileData GetTile(Vector2 coord)
        {
            foreach(TileData tile in tileData)
            {
                if (tile.coord == coord)
                    return tile;
            }

            return new TileData { coord = new Vector2(-1, -1) };
        }
    }

    [System.Serializable]
    public struct TileData
    {
        public Vector2 coord;
        public bool hasCustomInk;
        [ShowIf("hasCustomInk")]
        public Ink ink;
        [ShowIf("hasCustomInk")]
        public bool isPumpTile;

        public bool isPivotTile;
        [ShowIf("isPivotTile")]
        public Swipe.Direction PivotDirection;
        

    }
}