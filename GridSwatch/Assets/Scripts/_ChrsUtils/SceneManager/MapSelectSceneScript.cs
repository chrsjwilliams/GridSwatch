using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using GameScreen;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Analytics;

public class MapSelectSceneScript : Scene<TransitionData>
{

    [SerializeField] private Transform _mapContent;
    [SerializeField] private MapButton _mapButtonPrefab;

    [SerializeField] private Transform _tileTypeTestContent;


    internal override void OnEnter(TransitionData data)
    {
        // load levels
        foreach(MapData mapData in Services.MapManager.Maps)
        {
            MapButton mapButton = Instantiate(_mapButtonPrefab, _mapContent);
            bool finished = Convert.ToBoolean(PlayerPrefs.GetInt(mapData.name));
            MapButton.MapStatus status = finished ? MapButton.MapStatus.COMPLETED : MapButton.MapStatus.NOT_COMPLETED;
            mapButton.Init(mapData, status);
            mapButton.Pressed += OnMapSelected;
        }

        foreach (MapData mapData in Services.MapManager.TileTestMaps)
        {
            MapButton mapButton = Instantiate(_mapButtonPrefab, _tileTypeTestContent);
            bool finished = Convert.ToBoolean(PlayerPrefs.GetInt(mapData.name));
            MapButton.MapStatus status = finished ? MapButton.MapStatus.COMPLETED : MapButton.MapStatus.NOT_COMPLETED;
            mapButton.Init(mapData, status);
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
        CustomEvent mapStartedEvent = new CustomEvent("Map_Selected")
        {
            { "map_name", tData.SelecetdMap.mapName },
        };
        
        AnalyticsService.Instance.RecordEvent(mapStartedEvent);
        Services.Scenes.Swap<GameSceneScript>(tData);
    }

    public void BackButtonPressed()
    {
        Services.Scenes.Swap<TitleSceneScript>();
    }
}
