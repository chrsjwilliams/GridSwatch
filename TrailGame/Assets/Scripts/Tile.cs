using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public MapCoord coord { get; protected set; }
    private TaskManager _tm = new TaskManager();

    public bool canTraverse { get; protected set; }

    public void Init(MapCoord c, bool _canTraverse = true)
    {
        coord = c;
        canTraverse = _canTraverse;
    }

    public void SetTraversal(bool b){ canTraverse = b; }


    // Update is called once per frame
    void Update()
    {
        _tm.Update();
    }
}
