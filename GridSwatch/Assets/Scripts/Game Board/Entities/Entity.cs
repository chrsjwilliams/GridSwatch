using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{

    [SerializeField]
    protected Swipe swipe;
    public Direction direction { get; set; }

    [SerializeField]
    protected SpriteRenderer Sprite;

    public float moveSpeed { get; protected set; }
    public float arriveSpeed { get; protected set; }
    [SerializeField] protected int swipeCount = 0;
    public int SwipeCount
    {
        get { return swipeCount; }
    }

    public Ink Ink { get; set; }
    public ColorMode PrevColorMode { get; set; }
    public ColorMode CurrentColorMode { get; set; }

    public bool receiveInput { get; set; }
    public bool canMove { get; set; }
    public MapCoord coord { get; set; }
    public abstract void Init(MapCoord c);
    public abstract void PivotDirection(Direction d);
    public abstract void SetIndicators(Color c, int swipeCount = 0);
    public abstract void ResetIntensitySwipes();
    public abstract void Show(bool show);

}
