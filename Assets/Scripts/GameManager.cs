using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public enum GameProcedure
    {
        GameInit,
        GameStart,
        Game,
        GameOver
    }

    public static class GameManager
    {
        private static GameProcedure _gameProcedure;

        public static GameProcedure CurrentProcedure
        {
            get => _gameProcedure;
            set
            {
                _gameProcedure = value;

                _gameInitPanel.SetActiveEx(_gameProcedure == GameProcedure.GameInit);
                _gameStartPanel.SetActiveEx(_gameProcedure == GameProcedure.GameStart);
                _gamePanel.SetActiveEx(_gameProcedure == GameProcedure.Game ||
                                       _gameProcedure == GameProcedure.GameOver);
            }
        }

        public static List<EventData> EventQueue { get; set; } = new List<EventData>();
        private static Player _player;

        private static GameObject _gameInitPanel;
        private static GameObject _gameStartPanel;
        private static GameObject _gamePanel;

        public static IEnumerator Init()
        {
            // 初始化游戏
            _gameInitPanel = GameObject.Find("GameInit");
            _gameStartPanel = GameObject.Find("GameStart");
            _gamePanel = GameObject.Find("Game");
            CurrentProcedure = GameProcedure.GameInit;

            yield return null;

            // 初始化资源
            var request = Resources.LoadAsync<TextAsset>("data");
            yield return request;

            var textAsset = (TextAsset)request.asset;
            
            Debug.Log(textAsset.text);
            _player = new Player();

            CurrentProcedure = GameProcedure.GameStart;
            yield return null;
        }
    }
}