using UnityEngine;
using DG.Tweening;

namespace GameData
{
    public class PumpTile : Tile
    {
        public ColorMode PumpColor;


        public void Init(MapCoord mapCoord, Tile tile, Ink initInk, bool _canTraverse = true)
        {
            Coord = mapCoord;
            canTraverse = _canTraverse;
            PumpColor = initInk.colorMode;
            sr = tile.Sprite;
            tileInk = initInk;
            //SetColor(initInk);

            pumpIndicator = tile.PumpIndicator;

            sr.DOColor(Color.white, 0.0f).SetEase(Ease.InCubic);
            pumpIndicator.DOColor(tileInk.color, 0.0f).SetEase(Ease.InCubic);
        }

        public override void SetColor(Ink ink, bool isInit = false)
        {
        }

        protected override void TriggerEnterEffect(Entity entity)
        {
            entity.Ink = tileInk;
            entity.CurrentColorMode = PumpColor;
            entity.SetIndicators(Services.ColorManager.ColorScheme.GetColor(PumpColor)[0]);
            entity.ResetIntensitySwipes();
            entity.PrevColorMode = PumpColor;
        }

        protected override void TriggerExitEffect(Entity entity)
        {

        }
    }
}