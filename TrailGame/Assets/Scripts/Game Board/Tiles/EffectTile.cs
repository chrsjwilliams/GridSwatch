using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public enum TileEffect { NONE, PUMP, PIVOT, INVERT, SPLASH, FISSURE, DIM}

    public abstract class EffectTile : Tile
    {
        [SerializeField] protected TileEffect _primaryEffect;
        public TileEffect PrimaryEffect { get { return _primaryEffect; } }

        [SerializeField] protected TileEffect _secondaryEffect;
        public TileEffect SecondaryEffect { get { return _secondaryEffect; } }

        public abstract void TriggerEffect();
    }
}