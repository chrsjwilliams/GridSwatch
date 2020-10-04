using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public MapCoord coord { get; protected set; }
    private TaskManager _tm = new TaskManager();

    public bool canTraverse { get; protected set; }

    public Color CurrentColor { get; protected set; }
    [SerializeField] private SpriteRenderer sr;

    public void Init(MapCoord c, Color initColor,bool _canTraverse = true)
    {
        coord = c;
        canTraverse = _canTraverse;
        SetColor(initColor);
    }

    public void SetTraversal(bool b){ canTraverse = b; }


    public void SetColor(Color c)
    {
        // TODO: Fade in color effect!
        CurrentColor = c;
        sr.color = CurrentColor;
    }

    // Update is called once per frame
    void Update()
    {
        _tm.Update();
    }
}
