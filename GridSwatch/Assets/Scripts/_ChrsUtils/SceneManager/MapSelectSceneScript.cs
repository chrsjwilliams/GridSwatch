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
    [SerializeField] private SnapToItem _snapToItem;
    [SerializeField] private MapButton _mapButtonPrefab;
    [SerializeField] private MapPage _mapPagePrefab;
    [SerializeField] private Transform _tileTypeTestContent;

    private List<MapPage> mapPages = new List<MapPage>();

    private bool _inGame = false;

    internal override void OnEnter(TransitionData data)
    {
        _inGame = data != null && data.SelecetdMap != null;
        
        // how many map pages?
        int numMapPages = (int)Mathf.Ceil((float)Services.MapManager.Maps.Count / (float)MapPage.MAX_MAPS_PERPAGE);
        
        // Create mapPages and add them to content

        for (int i = 0; i < numMapPages; i++)
        {
            MapPage mapPage = Instantiate(_mapPagePrefab, _snapToItem.ContentPanel);
            mapPage.transform.localScale = Vector3.one;
            mapPages.Add(mapPage);
        }
        
        _snapToItem.Init(_mapPagePrefab.GetComponent<RectTransform>(), numMapPages);

        int pageIndex = 0;
        int addedMapCount = 0;
        // load levels
        foreach(MapData mapData in Services.MapManager.Maps)
        {
            MapButton mapButton = Instantiate(_mapButtonPrefab, mapPages[pageIndex].transform);
            mapPages[pageIndex].AddMapToPage(mapButton);
            bool finished = Convert.ToBoolean(PlayerPrefs.GetInt(mapData.name));
            MapButton.MapStatus status = finished ? MapButton.MapStatus.COMPLETED : MapButton.MapStatus.NOT_COMPLETED;
            mapButton.Init(mapData, status);
            mapButton.Pressed += OnMapSelected;

            
            addedMapCount++;
            if (addedMapCount % MapPage.MAX_MAPS_PERPAGE == 0)
            {
                pageIndex++;
                addedMapCount = 0;
            }
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
        if (_inGame)
        {
            Services.Scenes.PopScene(tData);
        }
        else
        {
            Services.Scenes.Swap<GameSceneScript>(tData);
        }
    }

    public void BackButtonPressed()
    {
        Services.Scenes.Swap<TitleSceneScript>();
    }

    public void CloseMapSelect()
    {
        Services.Scenes.PopScene();
    }
}
