using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{

    [SerializeField]
    protected Swipe swipe;
    public Swipe.Direction direction { get; protected set; }

    public float moveSpeed { get; protected set; }
    public float arriveSpeed { get; protected set; }

    public Ink Ink { get; set; }
    public ColorMode PrevColorMode { get; set; }
    public ColorMode CurrentColorMode { get; set; }

    public bool receiveInput { get; set; }
    public bool canMove { get; protected set; }
    public MapCoord coord { get; protected set; }
    public abstract void Init(MapCoord c);
    public abstract void PivotDirection(Swipe.Direction d);
    public abstract void SetIndicators(Color c);
    public abstract void ResetIntensitySwipes();


}
