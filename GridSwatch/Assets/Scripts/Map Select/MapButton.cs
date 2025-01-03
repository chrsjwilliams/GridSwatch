using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// We need a level button manager
// how does mapdtat gets passed to the button?

public class MapButton : MonoBehaviour
{
    public enum MapStatus {LOCKED, NOT_COMPLETED, COMPLETED}

    public MapData MapData { get; private set; }
    public MapStatus Status { get; private set; }

    [SerializeField] private TextMeshProUGUI _mapName;
    [SerializeField] private Image _mapIcon;
    [SerializeField] private Sprite _locked;
    [SerializeField] private Sprite _completed;

    public Action<MapData> Pressed;

    public void Init(MapData data, MapStatus status)
    {
        Status = status;
        MapData = data;
        _mapName.text = MapData.mapName.Replace("Map", "");
        UpdateIcon();
    }

    public void UpdateIcon()
    {
        if (Status == MapStatus.LOCKED)
        {
            _mapIcon.sprite = _locked;
            _mapIcon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            _mapIcon.raycastTarget = true;
        }
        else if (Status == MapStatus.NOT_COMPLETED)
        {
            _mapIcon.color = new Color(0.5f, 0.5f, 0.5f, 0f);
            _mapIcon.raycastTarget = false;

        }
        else
        {
            _mapIcon.sprite = _completed;
            _mapIcon.color = Services.ColorManager.GetColor(ColorMode.GREEN);
            _mapIcon.raycastTarget = false;
        }
    }
    
    public void OnPressed()
    {
        Pressed?.Invoke(MapData);
    }
}
