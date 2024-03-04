using UnityEngine;
using GameData;
using System;

namespace GameScreen
{
    public class GameSceneScript : Scene<TransitionData>
    {
        public bool endGame;

        public static bool hasWon { get; private set; }

        public const int LEFT_CLICK = 0;
        public const int RIGHT_CLICK = 1;

        public MapData MapData;

        public GameBoard board;

        public Player player;

        [SerializeField] MonoTweener swipeTextPulse;

        TaskManager _tm = new TaskManager();

        GameScreenUIController uIController;


        [SerializeField] private Transform _brushStrokeHolder;
        public Transform BrushStrokeHolder
        {
            get { return _brushStrokeHolder; }
        }

        bool finished;

        private void Awake()
        {
            uIController = GetComponent<GameScreenUIController>();
        }

       
        private void Start()
        {
            Services.EventManager.Register<SwipeEvent>(OnSwipe);

        }

        private void OnDestroy()
        {
            Services.EventManager.Unregister<SwipeEvent>(OnSwipe);

        }

        private void OnSwipe(SwipeEvent e)
        {
            swipeTextPulse.Kill();
        }

        internal override void OnEnter(TransitionData data)
        {
            if (data == null || data.SelecetdMap == null) return;

            swipeTextPulse?.Play();
            finished = false;
            MapData = data.SelecetdMap;
            Services.GameScene = this;
            Services.Board = board;

            Services.Board.CreateBoard(MapData);
            uIController.SetGameUI(MapData);

            Services.CameraController.AdjustCameraToGameBoard(board.Width, board.Height);

            player = Instantiate<Player>(Services.Prefabs.Player);
            player.transform.parent = transform;
            MapCoord startCoord = new MapCoord(MapData.PlayerStartPos.x, MapData.PlayerStartPos.y);
            player.Init(startCoord);
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

        public void GoToMapSelect()
        {
            Services.Scenes.Swap<MapSelectSceneScript>();
        }

        public void RestartMap()
        {
            Destroy(player.gameObject);
            board.ResetMap();

            swipeTextPulse?.Play();
            finished = false;
            Services.GameScene = this;
            Services.Board = board;

            Services.Board.CreateBoard(MapData);
            //uIController.SetGameUI(MapData);

            Services.CameraController.AdjustCameraToGameBoard(board.Width, board.Height);

            player = Instantiate<Player>(Services.Prefabs.Player);
            player.transform.parent = transform;
            MapCoord startCoord = new MapCoord(MapData.PlayerStartPos.x, MapData.PlayerStartPos.y);
            player.Init(startCoord);
        }

        private void EndGame()
        {
            Services.AudioManager.FadeAudio();

        }

        public void EndTransition()
        {

        }

        private void Update()
        {
            if (player == null || uIController == null) return;
            if (finished) return;

            if (!player.isMoving && uIController.IsGoalMet())
            {
                finished = true;
                MapData.finished = true;
                PlayerPrefs.SetInt(MapData.name, Convert.ToInt32(finished));
                PlayerPrefs.Save();
                Debug.Log("SAVE PLAYER PREFS " + MapData.name);
            }
        }
    }
}