using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    private Vector3 defaultPos = new Vector3(1, 1, -10);
    private TaskManager _tm = new TaskManager();

    public void Init()
    {
        _camera = GetComponent<Camera>();
    }

    public void ResetCamera()
    {
        _camera.orthographicSize = 5;
        transform.position = defaultPos;
    }


    public void AdjustCameraToGameBoard(int width, int height)
    {
        // x maps from 1 to 7 in a range from 3 to 15
        // y maps from 1 to 7 in a range from 3 to 15
        // size maps from 5 to 12 in a range from 3 to 15
        float x = Remap(width, 3, 15, 1, 7);
        float y = Remap(height, 3, 15, 1, 7);
        int tempSize = width > height ? width : height;
        float offset = tempSize;
        if(width != height)
            offset = width > height ? height : width;

        float size = Remap(tempSize, 3, 15, 5, 13);

        transform.position = new Vector3(x, y, -10);
        _camera.orthographicSize = size;
    }

    // Update is called once per frame
    void Update()
    {
        _tm.Update();
    }

    float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }
}
