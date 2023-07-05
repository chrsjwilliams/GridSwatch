using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectSceneScript : Scene<TransitionData>
{

    [SerializeField] Transform levelContent;

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
