using UnityEngine;

[System.Serializable]
public struct MapCoord
{

    public static readonly MapCoord ZERO = new MapCoord(0, 0);
    public static readonly MapCoord LEFT = new MapCoord(-1, 0);
    public static readonly MapCoord RIGHT = new MapCoord(1, 0);
    public static readonly MapCoord DOWN = new MapCoord(0, -1);
    public static readonly MapCoord UP = new MapCoord(0, 1);
    public int x;
    public int y;

    public MapCoord(int x_, int y_)
    {
        x = x_;
        y = y_;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        MapCoord coord = (MapCoord)obj;
        return (x == coord.x) && (y == coord.y);
    }

    public bool Equals(MapCoord coord)
    {
        return Equals(coord as object);
    }

    public override int GetHashCode()
    {
        return x ^ y;
    }

    public static bool operator ==(MapCoord a, MapCoord b)
    {
        if (ReferenceEquals(a, b)) return true;
        if ((object)a == null || (object)b == null) return false;
        return (a.x == b.x) && (a.y == b.y);
    }

    public static bool operator !=(MapCoord a, MapCoord b)
    {
        return !(a == b);
    }

    public int Distance(MapCoord otherCoord)
    {
        return Mathf.Abs(x - otherCoord.x) + Mathf.Abs(y - otherCoord.y);
    }

    public static int Distance(MapCoord a, MapCoord b)
    {
        return a.Distance(b);
    }

    public MapCoord Add(MapCoord otherCoord)
    {
        return new MapCoord(x + otherCoord.x, y + otherCoord.y);
    }

    public static MapCoord Add(MapCoord a, MapCoord b)
    {
        return a.Add(b);
    }

    public MapCoord Multiply(int i)
    {
        return new MapCoord(x * i, y * i);
    }

    public static MapCoord Multiply(int i, MapCoord a)
    {
        return a.Multiply(i);
    }

    public MapCoord Subtract(MapCoord otherCoord)
    {
        return Add(otherCoord.Multiply(-1));
    }

    public static MapCoord Subtract(MapCoord a, MapCoord b)
    {
        return a.Subtract(b);
    }

    public Vector3 WorldPos()
    {
        return new Vector3(x, y, 0);
    }

    public static MapCoord[] Directions()
    {
        MapCoord[] directions = new MapCoord[4]
        {
            new MapCoord(1, 0),
            new MapCoord(0, 1),
            new MapCoord(-1, 0),
            new MapCoord(0, -1)
        };
        return directions;
    }

    public static float DirectionToAngle(MapCoord direction)
    {
        for (int i = 0; i < 4; i++)
        {
            if (direction == Directions()[i])
            {
                return i * 90;
            }
        }
        return 0;
    }

    public override string ToString()
    {
        return "Tile Coord| X: " + x + ", Y: " + y;
    }
}
