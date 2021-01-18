using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public MapCoord coord { get; protected set; }
    private TaskManager _tm = new TaskManager();

    public bool canTraverse { get; protected set; }

    public Ink tileInk { get; protected set; }
    public ColorMode CurrentColorMode { get; protected set; }
    public Color CurrentColor { get; protected set; }
    [SerializeField] protected SpriteRenderer sr;

    public virtual void Init(MapCoord c, Ink initInk,bool _canTraverse = true)
    {
        coord = c;
        canTraverse = _canTraverse;
        sr = GetComponent<SpriteRenderer>();
        tileInk = new Ink();
        SetColor(initInk, true);
    }

    public void SetTraversal(bool b){ canTraverse = b; }


    public void SetColor(Ink ink, bool isInit = false)
    {
        if (isInit)
            tileInk = ink;
        // TODO: Fade in color effect!

        if (ShouldMixColors(ink))
        {
            Services.Board.CurrentFillAmount[(int)tileInk.colorMode]--;
            tileInk = Services.ColorManager.MixColors(tileInk, ink);
        }
        else if (CanOverwriteColor(ink))
        {
            if (!(this is PumpTile))
            {
                if (ink.colorMode != tileInk.colorMode)
                    Services.Board.CurrentFillAmount[(int)tileInk.colorMode]--;
            }
            tileInk = ink;
            
        }
        if(tileInk.colorMode != CurrentColorMode && !(this is PumpTile))
            Services.Board.CurrentFillAmount[(int)tileInk.colorMode]++;

        CurrentColorMode = tileInk.colorMode;

        sr.color = tileInk.color;
        

    }

    // QUESTION: Should CYAN overwrite GREEN? Leaning NO


    public bool CanOverwriteColor(Ink newInk)
    {
        return  //  Tile has no color
                (tileInk.colorMode == ColorMode.NONE) ||
                //  Tile is not Black Color
                (tileInk.colorMode != ColorMode.BLACK) ||
                //  New color has higher intensity
                (newInk.colorMode == tileInk.colorMode && newInk.Intensity > tileInk.Intensity);
                //  Should lower intensity colors mix to make higher intensity colors?
    }

    public bool ShouldMixColors(Ink newInk)
    {
        return  tileInk.colorMode != ColorMode.NONE &&
                newInk.colorMode != ColorMode.NONE &&
                tileInk.colorMode != newInk.colorMode;
    }


    // Update is called once per frame
    void Update()
    {
        _tm.Update();
    }
}
