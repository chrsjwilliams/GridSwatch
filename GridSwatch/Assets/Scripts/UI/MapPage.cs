using System.Collections.Generic;
using UnityEngine;

public class MapPage : MonoBehaviour
{
    public const int MAX_MAPS_PERPAGE = 20;
    private List<MapButton> maps = new List<MapButton>();

    public void AddMapToPage(MapButton map)
    {
        maps.Add(map);
    }
    
}
