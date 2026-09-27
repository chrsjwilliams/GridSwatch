using System;
using System.Collections.Generic;
using System.IO;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using Application = UnityEngine.Application;

public class SaveDataJSON : MonoBehaviour
{

    private string m_SaveFilePath;
    public static SaveDataJSON Instance { get; private set; }
    
    private void Awake()
    {
        m_SaveFilePath = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        Services.EventManager.Register<SaveDataEvent>(SaveData);
        LoadData();
    }
    

    private void OnDestroy()
    {
        Services.EventManager.Unregister<SaveDataEvent>(SaveData);
    }

    public void SaveData(SaveDataEvent e)
    {
        string json = JsonUtility.ToJson(e.SavedData, true);
        File.WriteAllText(m_SaveFilePath, json);
        Debug.Log("[SAVEDATA] " + json);
    }

    public void LoadData()
    {

        if (File.Exists(m_SaveFilePath))
        {
            string json = File.ReadAllText(m_SaveFilePath);
            
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("[SAVE DATA MANAGER] Data loaded successfully!");
            Services.EventManager.Fire(new LoadDataEvent(data));
        }
        else
        {
            Debug.Log("[SAVE DATA MANAGER] No Data found. Creating now...");
            Services.EventManager.Fire(new CreateSaveDataEvent());
        }
    }
}
