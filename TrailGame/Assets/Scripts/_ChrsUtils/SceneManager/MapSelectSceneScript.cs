using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectSceneScript : Scene<TransitionData>
{

    [SerializeField] Transform mapContent;

    internal override void OnEnter(TransitionData data)
    {
        // load levels
    }

    internal override void OnExit()
    {

    }

    public void BackButtonPressed()
    {
        Services.Scenes.Swap<TitleSceneScript>();
    }
}
