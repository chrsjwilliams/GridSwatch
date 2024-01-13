using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushStroke : MonoBehaviour
{
    private LineRenderer lr;
    private List<Vector3> points = new List<Vector3>();
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    public void Init(Color c, Vector3 _startPos, Vector3 _endPos)
    {
        lr = GetComponent<LineRenderer>();
        startPos = _startPos;
        endPos = _endPos;
        
        points.Add(startPos);
        points.Add(endPos);
        SetColor(c);
        for (int i = 0; i < points.Count; i++)
        {
            lr.SetPosition(i, points[i]);
            
        }
    }

    public void SetColor(Color c)
    {
        lr.startColor = c;
        lr.endColor = c;
    }

    public void SetEndPos(Vector3 _endPos)
    {
        endPos = _endPos;
        points[1] = _endPos;
    }
    
    private void Update()
    {
        if (lr == null) return;
        
        for (int i = 0; i < points.Count; i++)
        {
            lr.SetPosition(i, points[i]);
            
        }
    }
    
}
