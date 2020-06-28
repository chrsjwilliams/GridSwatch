using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private TaskManager _tm = new TaskManager();

    public void Init()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _tm.Update();
    }
}
