using UnityEngine;


public class TransitionData
{
    private static TransitionData instance;
    public static TransitionData Instance
    {
        get
        {
            if (instance == null)
                instance = new TransitionData();

            return instance;
        }
        set { }
    }

    public GameData.MapData SelecetdMap;
}

