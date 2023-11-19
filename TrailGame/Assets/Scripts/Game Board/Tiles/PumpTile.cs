using UnityEngine;

namespace GameData
{
    public class PumpTile : Tile
    {
        public ColorMode PumpColor;

        private TaskManager _tm = new TaskManager();

        public override void Init(MapCoord c, Ink initInk, bool _canTraverse = true)
        {
            coord = c;
            canTraverse = _canTraverse;
            PumpColor = initInk.colorMode;
            sr = GetComponent<SpriteRenderer>();
            tileInk = new Ink();
            SetColor(initInk);

            pumpIndicator = GetComponentsInChildren<SpriteRenderer>()[1];


            sr.color = Color.white;
            pumpIndicator.color = tileInk.color;
        }

        // Update is called once per frame
        void Update()
        {
            _tm.Update();
        }
    }
}