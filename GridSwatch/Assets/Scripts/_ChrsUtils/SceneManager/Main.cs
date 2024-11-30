using System;
using Sirenix.Utilities;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Analytics;

#region Main.cs Overview
/************************************************************************************************************************/
/*                                                                                                                      */
/*     Seneca starts running fdorm this script.                                                                         */
/*                                                                                                                      */
/*    Main.cs is responsible  for:                                                                                      */
/*    Instantiating the ScenePrefab Database which holds a prefab for all scenes in the game.                           */
/*    Initalizing the Game Event Manager                                                                                */
/*    Initalizing  the scene manager                                                                                    */
/*                                                                                                                      */
/*    And                                                                                                               */
/*                                                                                                                      */
/*    Loading the first scene.                                                                                          */
/*                                                                                                                      */
/************************************************************************************************************************/
#endregion

#region Quick Overview of Prefab Database
/************************************************************************************************************************/
/*                                                                                                                      */
/*    Prefab Database is found in: ChrsUtils -> SceneManager-> SceneManager.cs                                          */
/*                                                                                                                      */
/*        This holds the prefabs for all scenes in the game.                                                            */
/*                                                                                                                      */
/************************************************************************************************************************/
#endregion

#region Quick Overview of Game Event System
/************************************************************************************************************************/
/*                                                                                                                      */
/*     Event System is found in: ChrsUtils -> EventsManager                                                             */
/*                                                                                                                      */
/*        Overview:                                                                                                     */
/*        If we want a script to respond to a thing that happens in another script without coupling                     */
/*        the scripts together, we use the Events Manager.                                                              */
/*                                                                                                                      */
/*        How it works:                                                                                                 */
/*        We register a delegate to a GameEvent  (GameEvents are found in SenecaEvent.cs). When we                      */
/*         fire that event using the Event Manager, the delegate is executed by the computer                            */
/*                                                                                                                      */
/************************************************************************************************************************/
#endregion

#region Quick Overview of SceneManagement System
/************************************************************************************************************************/
/*                                                                                                                      */
/*       Event System is found in: ChrsUtils -> SceneManager-> SceneManager.cs                                          */
/*                                                                                                                      */
/*      Overview:                                                                                                       */
/*       Loads and unloads scenes as prefabs. This is done in this project to avoid the use of singletons.              */
/*                                                                                                                      */
/*       How it works:                                                                                                  */
/*       SceneManagement system works as a stack where scene prefabs are pushed, popped, and swapped.                   */
/*       Each scene passes Transition data to the next scene. In the case of Seneca the data being passed if            */
/*       whether you've been to the scene or not.                                                                       */
/*                                                                                                                      */
/************************************************************************************************************************/
#endregion
public class Main : MonoBehaviour
{
    [SerializeField] private bool DEBUG_MODE;
    [SerializeField] private ModalContent analyticsModalContent;
    private void Awake()
    {
        Assert.raiseExceptions = true;

        InitalizeServices();

        
    }
    

    private async void InitalizeServices()
    {
        Services.Main = this;
        var options = new InitializationOptions();
        options.SetEnvironmentName("development");
        
        // Initialize Unity Services
        await UnityServices.InitializeAsync(options);
        
        Services.EventManager = new GameEventsManager();
        Services.GeneralTaskManager = new TaskManager();
        Services.Prefabs = Resources.Load<PrefabDB>("Prefabs/PrefabDB");
        Services.InputManager = new InputManager();
        Services.GameManager = GetComponent<GameManager>();
        Services.GameManager.Init();
        Services.ColorManager = GetComponent<ColorManager>();
        Services.CameraController = Camera.main.GetComponent<CameraController>();
        Services.CameraController.Init();
        Services.AudioManager = GetComponent<AudioManager>();
        Services.MapManager = GetComponent<MapManager>();
        Services.MapManager.Init();
        Services.Scenes = new GameSceneManager<TransitionData>(gameObject, Services.Prefabs.Scenes);
        
        if(!DEBUG_MODE)
            Services.Scenes.PushScene<TitleSceneScript>();
    }

    private void Start()
    {
        /*
        string value = PlayerPrefs.GetString("enabled_analytics");
        bool enabledAnalytics = false;
        if (!string.IsNullOrEmpty(value))
        {
            enabledAnalytics = bool.Parse(value);
        }

        if (!enabledAnalytics)
        {
            ModalControl.Instance.DisplayQuestion(analyticsModalContent, (response) =>
            {
                enabledAnalytics = response;

                if (enabledAnalytics)
                {
                    AnalyticsService.Instance.StartDataCollection();
                    Debug.Log("Start Collecting");
                }
                
                PlayerPrefs.SetString("enabled_analytics", enabledAnalytics.ToString());
                PlayerPrefs.Save();
            });
        }
        else
        {
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Start Collecting");
        }
        */
    }
}
