﻿using GameData;
using GameScreen;

public class Services
{
    public static Main Main { get; set; }
    public static GameManager GameManager { get; set; }

    public static AudioManager AudioManager { get; set; }
    public static GameEventsManager EventManager { get; set; }
    public static TaskManager GeneralTaskManager { get; set; }
    public static PrefabDB Prefabs { get; set; }
    public static ColorManager ColorManager { get; set; }

    public static InputManager InputManager { get; set; }
    public static MapManager MapManager { get; set; }
    public static GameSceneManager<TransitionData> Scenes { get; set; }

    public static GameSceneScript GameScene { get; set; }
    public static CameraController CameraController { get; set; }
    public static GameBoard Board { get; set; }
}
