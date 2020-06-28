using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameSceneScript : Scene<TransitionData>
{
    public bool endGame;

    public static bool hasWon { get; private set; }

    public const int LEFT_CLICK = 0;
    public const int RIGHT_CLICK = 1;

    public GameBoard board;

    TaskManager _tm = new TaskManager();

    // Height Range: 3 - 15
    // Width Range: 3 - 9
    private void Start()
    {
        Services.GameScene = this;
        Services.Board = board;

        Services.Board.CreateBaord(9, 15);
        Services.CameraController.AdjustCameraToGameBoard(board.Width, board.Height);
    }

    internal override void OnEnter(TransitionData data)
    {
    }

    public void EnterScene()
    {
        

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
	}
}
