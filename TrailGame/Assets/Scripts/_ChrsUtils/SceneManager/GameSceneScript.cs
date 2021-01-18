using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.UI;

public class GameSceneScript : Scene<TransitionData>
{
    public bool endGame;

    public static bool hasWon { get; private set; }

    public const int LEFT_CLICK = 0;
    public const int RIGHT_CLICK = 1;

    public MapData MapData;

    public GameBoard board;

    public Player player;

    TaskManager _tm = new TaskManager();


    // TODO: Find a way to evenly space out color goals

    public TextMeshProUGUI[] colorGoalTextUI;
    public Image[] colorGoalIconUI;

    // Height Range: 3 - 15
    // Width Range: 3 - 9
    private void Start()
    {
        /*
        Services.GameScene = this;
        Services.Board = board;

        Services.Board.CreateBoard(MapData);
        //Services.Board.CreateBaord(3, 3);
        Services.CameraController.AdjustCameraToGameBoard(board.Width, board.Height);

        player = Instantiate<Player>(Services.Prefabs.Player);
        player.transform.parent = transform;
        MapCoord startCoord = new MapCoord(MapData.PlayerStartPos.x, MapData.PlayerStartPos.y);
        player.Init(startCoord);
        */
    }

    internal override void OnEnter(TransitionData data)
    {
        Services.GameScene = this;
        Services.Board = board;

        Services.Board.CreateBoard(MapData);
        PrepGameUI();
        //Services.Board.CreateBaord(3, 3);
        Services.CameraController.AdjustCameraToGameBoard(board.Width, board.Height);

        player = Instantiate<Player>(Services.Prefabs.Player);
        player.transform.parent = transform;
        MapCoord startCoord = new MapCoord(MapData.PlayerStartPos.x, MapData.PlayerStartPos.y);
        player.Init(startCoord);
    }

    public void EnterScene()
    {
        

    }

    public void PrepGameUI()
    {
        for(int i = 0; i < MapData.colorGoals.Length; i++)
        {
            colorGoalIconUI[i].color = Services.ColorManager.GetColor(MapData.colorGoals[i]);
            colorGoalTextUI[i].text =  board.CurrentFillAmount[(int)MapData.colorGoals[i]] + " / " + MapData.colorTileCountGoal[i];

        }
    }

    public void SwapScene()
    {
        Services.AudioManager.SetVolume(1.0f);
        Services.Scenes.Swap<TitleSceneScript>();
    }

    public void SceneTransition()
    {
        _tm.Do
        (
            new ActionTask(SwapScene)
        );
    }

    private void EndGame()
    {
        Services.AudioManager.FadeAudio();

    }

    public void EndTransition()
    {

    }
    
	// Update is called once per frame
	void Update ()
    {
        _tm.Update();
        for (int i = 0; i < MapData.colorGoals.Length; i++)
        {
            colorGoalTextUI[i].text = board.CurrentFillAmount[(int)MapData.colorGoals[i]] + " / " + MapData.colorTileCountGoal[i];

        }
    }
}
