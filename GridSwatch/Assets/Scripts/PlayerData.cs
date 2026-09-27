using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    private static PlayerData _instance = null;

    public static PlayerData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerData();
            }

            return _instance;
        }
    }


    public List<LevelData> SavedLevelData;

    public PlayerData()
    {
        
    }

    public void RegiserEvents()
    {
        Services.EventManager.Register<CreateSaveDataEvent>(OnCreateSavedData);
        Services.EventManager.Register<LoadDataEvent>(OnLoadData);
    }

    private void OnCreateSavedData(CreateSaveDataEvent e)
    {
        Instance.SavedLevelData = new List<LevelData>();
    }

    public void WritePlayerData(string levelName, bool completed, int swipes)
    {
        if (!completed)
            return;
        
        LevelData data = new LevelData();
        data._levelname = levelName;
        data._completed = completed;
        data.minSwipes = swipes;

        bool SaveNew = true;
        bool overwrite = false;
        foreach (var levelData in Instance.SavedLevelData)
        {
            if (levelData._levelname == levelName)
            {
                SaveNew = false;
                if (levelData.minSwipes > swipes)
                {
                    overwrite = true;
                }
                break;
            }
        }

        if (SaveNew)
        {
            Instance.SavedLevelData.Add(data);
            Services.EventManager.Fire(new SaveDataEvent(this));
            return;
        }
        
        if (overwrite)
        {
            LevelData oldData = GetData(levelName);
            if (oldData != null)
            {
                int index = Instance.SavedLevelData.IndexOf(oldData);
                Instance.SavedLevelData[index] = data;
            }
            Services.EventManager.Fire(new SaveDataEvent(this));
        }
    }


    private void OnLoadData(LoadDataEvent e)
    {
        foreach (var data in e.LoadedData.SavedLevelData)
        {
           data.Print();
        }
        Instance.SavedLevelData = e.LoadedData.SavedLevelData;
    }
    

    public LevelData GetData(string levelName)
    {
        foreach (var levelData in Instance.SavedLevelData)
        {
            if (levelData._levelname == levelName)
                return levelData;
        }

        return null;
    }
    

    [System.Serializable]
    public class LevelData
    {
        public string _levelname;
        public bool _completed;
        public int minSwipes;

        public void Print()
        {
            Debug.Log("Level : " + _levelname + "\nCompleted: " + _completed + "\nMin Swipes: " + minSwipes);
        }
    }
}
