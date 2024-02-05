using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class GetMainCameraForCanvas : MonoBehaviour
{
    private Canvas _canvas;
    // Start is called before the first frame update
    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = Camera.main;
    }
}
