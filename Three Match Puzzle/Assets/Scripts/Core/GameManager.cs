using UnityEngine;
using Framework;
using System.Collections;
using Anipang.Common;

namespace Anipang
{
    public class GameManager : MonoBehaviour
    {
        // Game.
        [field: SerializeField]
        public GameConfigs GameConfigs { get; private set; }
        private GameEvents gameEvents;

        // UI.
        [field: SerializeField]
        public Transform UIRoot { get; private set; }

        // Board.
        [field: SerializeField]
        public Board Board { get; private set; }
        private BoardEvents boardEvents;

        //TODO: UI 매니저 필요함.
        //TODO: 오디오 매니저 필요함.
        //TODO: 이펙트 매니저 필요함.
        //TODO: 인풋 매니저 필요함.

        private IEnumerator Start()
        {
            yield return InitializeGame();
        }

        private IEnumerator InitializeGame()
        {
            var gameConfigsResourceRequest = Resources.LoadAsync<GameConfigs>(Defines.GAME_CONFIGS_PATH);
            yield return gameConfigsResourceRequest;
            GameConfigs = gameConfigsResourceRequest.asset as GameConfigs;
            

            UIRoot = GameObject.Find("Game Canvas").GetComponent<Transform>();

            var boardPrefabResourceRequest = Resources.LoadAsync<GameObject>(Defines.BOARD_PREFAB_PATH);
            yield return boardPrefabResourceRequest;
            var boardObj = Instantiate(boardPrefabResourceRequest.asset as GameObject, UIRoot);
            Board = boardObj.GetComponent<Board>();

            BoardEvents boardEvents = null;
            yield return Board.Initialize(GameConfigs.Row, GameConfigs.Column, 
                (events) => { boardEvents = events; });

            gameEvents = new GameEvents(boardEvents);

            yield return Board.ComposeBoard();

            yield return new WaitForSeconds(5);

            yield return Board.CheckMatchedBlocks();
            yield return Board.RemoveMatchedBlocks();

            // Board.Clear();
        }
    }
}
