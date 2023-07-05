using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    TaskManager tm = new TaskManager();

    public void Init()
    {
        
    }

    public void CreateLevel(LevelInfo info)
    {
        
    }

    void Update()
    {
        tm.Update();
    }
}

public struct LevelInfo
{

}
