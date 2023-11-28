using UnityEngine;
using DG.Tweening;

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
            tileInk = initInk;
            //SetColor(initInk);

            pumpIndicator = GetComponentsInChildren<SpriteRenderer>()[1];

            sr.DOColor(Color.white, 0.0f).SetEase(Ease.InCubic);
            pumpIndicator.DOColor(tileInk.color, 0.0f).SetEase(Ease.InCubic);
        }

        public override void SetColor(Ink ink, bool isInit = false)
        {
        }

        // Update is called once per frame
        void Update()
        {
            _tm.Update();
        }
    }
}