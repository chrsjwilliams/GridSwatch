using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData {
    [CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/MapData", order = 1)]
    public class MapData : ScriptableObject
    {
        public string mapName;
        public GameBoard.ColorType BoardType;
        public Vector2 BoardSize;

        public bool UseMagenta;
        public bool UseCyan;
        public bool UseYellow;

        public List<Vector2> ImpassableMapCoords;

        public Vector2 PlayerStartPos;

        public List<Vector2> PumpLocationsMagenta;
        public List<Vector2> PumpLocationsCyan;
        public List<Vector2> PumpLocationsYellow;

        public string MapGoal;

        public List<ColorGoal> colorGoals;

        [System.Serializable]
        public struct ColorGoal
        {
            public ColorMode colorMode;
            public int amount;
        }
}
}