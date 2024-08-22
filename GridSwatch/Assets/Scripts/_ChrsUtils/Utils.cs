using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static void Shuffle<T>(this IList<T> list, System.Random rnd)
    {
        for (var i = list.Count - 1; i >= 0; i--)
            list.Swap(i, rnd.Next(0, i));
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }

    public static void ShowGroup(this CanvasGroup group, bool show)
    {
        group.alpha = Convert.ToInt32(show);
        group.blocksRaycasts = show;
        group.interactable = show;
    }

    public static void ShowGroup(this CanvasGroup group, bool show, bool interactable)
    {
        group.alpha = Convert.ToInt32(show);
        group.blocksRaycasts = !show ? false : interactable;
        group.interactable = !show ? false : interactable;
    }

    public static void RefreshLayoutGroup(this HorizontalOrVerticalLayoutGroup group)
    {
        Canvas.ForceUpdateCanvases();
        group.enabled = false;
        group.enabled = true;
    }

    public static bool CoordsOverlap(this Vector3 v1, Vector3 v2)
    {
        return v1.x == v2.x && v1.y == v2.y;
    }
    
    public static float Map (this float from, float fromMin, float fromMax, float toMin,  float toMax)
    {
        var fromAbs  =  from - fromMin;
        var fromMaxAbs = fromMax - fromMin;      
       
        var normal = fromAbs / fromMaxAbs;
 
        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;
 
        var to = toAbs + toMin;
       
        return to;
    }
}