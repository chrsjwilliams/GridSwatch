using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    public enum Direction { NONE = 0, LEFT, RIGHT, DOWN, UP };

    private int currentMovementIndex = -1;
    private Direction[] movementArray = {   Direction.LEFT, Direction.RIGHT,
                                            Direction.DOWN, Direction.UP};
    public Direction RecordedDirection { get; private set; }
    public Direction CurrentDirection { get; private set; }

    private bool tap, swipeLeft, swipeRight, swipeDown, swipeUp;
    private bool isDragging = false;
    private const int ACTIVATION_RADIUS = 125;
    private Vector2 startTouch, swipeDelta;

    public bool Tap {get { return tap; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeDown { get { return swipeDown; } }
    public bool SwipeUp { get { return swipeUp; } }
    

    public Vector2 SwipeDelta { get { return swipeDelta; } }

    private void ResetInput() 
    {
        isDragging = false;
        startTouch = swipeDelta = Vector2.zero;
        RecordedDirection = Direction.NONE;
        currentMovementIndex = -1;
    }

    public void ResetRotations()
    {
        movementArray = new Direction[4]{   
            Direction.LEFT, Direction.RIGHT, Direction.DOWN, Direction.UP};
    }

    public void RotateInputs(int numRotations, bool rotateRight)
    {
        if(rotateRight) RotateRight(numRotations);
        else RotateLeft(numRotations);
    }

    private void Update()
    {
        tap = swipeLeft = swipeRight = swipeDown = swipeUp = false;

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDragging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //ResetInput();
        }
        #endregion

        #region Mobile Inputs
        if(Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended ||
                    Input.touches[0].phase == TouchPhase.Canceled)
            {
            }
        }
        #endregion

        swipeDelta = Vector2.zero;
        if(isDragging)
        {
            if(Input.touches.Length != 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButtonUp(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        if(swipeDelta.magnitude > ACTIVATION_RADIUS)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x < 0)
                {
                    swipeLeft = true;
                    RecordedDirection = Direction.LEFT;
                }
                else
                {
                    swipeRight = true;
                    RecordedDirection = Direction.RIGHT;
                }
            }
            else
            {
                if (y < 0)
                {
                    swipeDown = true;
                    RecordedDirection = Direction.DOWN;
                }
                else
                {
                    swipeUp = true;
                    RecordedDirection = Direction.UP;
                }
            }
            currentMovementIndex = (int)RecordedDirection - 1;
            CurrentDirection = movementArray[currentMovementIndex];
            Services.EventManager.Fire(new SwipeEvent(this));
            ResetInput();
        }
    }

    #region InputRotation
    private void RotateLeft(int numRotations)
    {
        int d = -1;
        int j;
        Direction temp;
        int numSets = GreatestCommonDivisor(movementArray.Length, numRotations);
        for (int i = 0; i < numSets; i++)
        {
            j = i;
            temp = movementArray[i];

            while(true)
            {
                d = (j + numRotations) % movementArray.Length;
                if(d == i)
                    break;

                movementArray[j] = movementArray[d];
                j = d;
            }
            movementArray[j] = temp;
        }
    }

    private int GreatestCommonDivisor(int a, int b)
    {
        if( b == 0) return a;
        else return GreatestCommonDivisor(b, a % b);
    }

    private void RotateRight(int numRotations)
    {
        numRotations = numRotations % movementArray.Length;
        Reverse(movementArray, 0, movementArray.Length - 1);
        Reverse(movementArray, 0, numRotations - 1);
        Reverse(movementArray, numRotations, movementArray.Length - 1);
    }

    private void Reverse(Direction[] arr, int start, int end)
    {
        while(start < end)
        {
            Direction temp = arr[start];
            arr[start] = arr[end];
            arr[end] = temp;
            start++;
            end--;
        }
    }
    #endregion
}
