using System.Collections;
using System.Collections.Generic;
using GameData;
using GameScreen;
using UnityEngine;

public class MapSelectSceneScript : Scene<TransitionData>
{

    [SerializeField] private Transform _mapContent;
    [SerializeField] private MapButton _mapButtonPrefab;

    internal override void OnEnter(TransitionData data)
    {
        // load levels
        foreach(MapData mapData in Services.MapManager.Maps)
        {
            MapButton mapButton = Instantiate(_mapButtonPrefab, _mapContent);
            mapButton.Init(mapData, MapButton.MapStatus.NOT_COMPLETED);
            mapButton.Pressed += OnMapSelected;
        }

    }

    internal override void OnExit()
    {

    }

    public void OnMapSelected(MapData data)
    {
        TransitionData tData = new TransitionData();
        tData.SelecetdMap = data;
        Services.Scenes.Swap<GameSceneScript>(tData);
    }

    public void BackButtonPressed()
    {
        Services.Scenes.Swap<TitleSceneScript>();
    }
}
