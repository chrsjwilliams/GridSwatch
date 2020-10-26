using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpTile : Tile
{
    public ColorMode PumpColor;

    private TaskManager _tm = new TaskManager();

    public void Init(MapCoord c, Ink initInk, bool _canTraverse = true)
    {
        coord = c;
        canTraverse = _canTraverse;
        PumpColor = initInk.colorMode;
        sr = GetComponent<SpriteRenderer>();
        SetColor(initInk);
    }

    // Update is called once per frame
    void Update()
    {
        _tm.Update();
    }
}
