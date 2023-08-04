using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// We need a level button manager
// how does mapdtat gets passed to the button?

public class LevelButton : MonoBehaviour
{
    public enum LevelStatus {LOCKED, NOT_COMPLETED, COMPLETED}

    public MapData MapData { get; private set; }
    public LevelStatus Status { get; private set; }

    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private Image _levelIcon;
    [SerializeField] private Sprite _locked;
    [SerializeField] private Sprite _completed;
    [SerializeField] private Button _levelButton;

    public void Init(MapData data, LevelStatus status)
    {
        Status = status;
        MapData = data;
        _levelName.text = MapData.levelName;
        UpdateIcon();
    }

    public void UpdateIcon()
    {
        if (Status == LevelStatus.LOCKED)
        {
            _levelButton.interactable = false;
            _levelIcon.sprite = _locked;
            _levelIcon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
        else if (Status == LevelStatus.NOT_COMPLETED)
        {
            _levelButton.interactable = true;
            _levelIcon.color = new Color(0.5f, 0.5f, 0.5f, 0f);
        }
        else
        {
            _levelButton.interactable = true;
            _levelIcon.sprite = _completed;
            _levelIcon.color = Color.white;
        }
    }
    
    
}
