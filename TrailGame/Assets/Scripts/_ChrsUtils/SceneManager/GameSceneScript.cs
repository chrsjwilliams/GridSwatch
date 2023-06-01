using UnityEngine;
using GameData;

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

        TaskManager _tm = new TaskManager();

        GameScreenUIController uIController;

        private void Awake()
        {
            uIController = GetComponent<GameScreenUIController>();
        }

        // Height Range: 3 - 15
        // Width Range: 3 - 9
        private void Start()
        {

            Services.GameScene = this;
            Services.Board = board;

            Services.Board.CreateBoard(MapData);
            uIController.SetGameUI(MapData);

            //Services.Board.CreateBaord(3, 3);
            Services.CameraController.AdjustCameraToGameBoard(board.Width, board.Height);

            player = Instantiate<Player>(Services.Prefabs.Player);
            player.transform.parent = transform;
            MapCoord startCoord = new MapCoord(MapData.PlayerStartPos.x, MapData.PlayerStartPos.y);
            player.Init(startCoord);

        }

        internal override void OnEnter(TransitionData data)
        {
            Services.GameScene = this;
            Services.Board = board;
            Debug.Log("~~~~ MAKE BOARD");
            Services.Board.CreateBoard(MapData);
            uIController.SetGameUI(MapData);

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

        private void Update()
        {
            if(!player.isMoving && uIController.IsGoalMet())
            {
                Debug.Log("GAME WON!");
            }
        }
    }
}